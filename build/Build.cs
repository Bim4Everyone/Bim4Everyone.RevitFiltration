using System;
using System.IO;
using System.Linq;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Components;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

class Build : NukeBuild {
    [Solution]
    readonly Solution Solution;

    [Parameter]
    readonly AbsolutePath ArtifactPath;

    [Parameter]
    readonly AbsolutePath TestResultsPath;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter]
    readonly AbsolutePath DocsCaches = RootDirectory / Path.Combine("docs", "api");

    [Parameter]
    readonly string DocsConfig = Path.Combine("docs", "docfx.json");

    [Parameter]
    readonly string DocsOutput = Path.Combine("docs", "_site");

    [Parameter]
    readonly AbsolutePath PublishOutput;

    [Parameter]
    readonly AbsolutePath pyRevitOutput;

    [Parameter]
    readonly AbsolutePath Bim4EveryoneOutput;

    [Parameter("Build Revit versions.")]
    readonly int[] RevitVersions = [2020, 2021, 2022, 2023, 2024];

    public Build() {
        ArtifactPath = RootDirectory / "bin";
        TestResultsPath = RootDirectory / "TestResults";
        
        AbsolutePath appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        pyRevitOutput = appdataFolder / "pyRevit-Master";
        Bim4EveryoneOutput = appdataFolder / "pyRevit" / "Extensions" / "BIM4Everyone.lib";
        PublishOutput = appdataFolder / "pyRevit" / "Extensions" / "BIM4Everyone.lib" / "dosymep_libs" / "libs";
    }

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    Target Clean =>
        _ => _
            .Before(Restore)
            .Executes(() => {
                ArtifactPath.CreateOrCleanDirectory();
                TestResultsPath.CreateOrCleanDirectory();
                (RootDirectory / DocsOutput).CreateOrCleanDirectory();
                DocsCaches.GlobFiles("**/*.yml").DeleteFiles();
                RootDirectory.GlobDirectories("**/bin", "**/obj")
                    .Where(item => item != RootDirectory / "build" / "bin")
                    .Where(item => item != RootDirectory / "build" / "obj")
                    .DeleteDirectories();
            });

    Target Restore =>
        _ => _
            .DependsOn(Clean)
            .Executes(() => {
                DotNetRestore(s => s
                    .SetProjectFile(Solution));
            });

    Target DownloadBim4Everyone =>
        _ => _
            .OnlyWhenStatic(() => IsServerBuild)
            .Executes(() => {
                // потому что основные пакеты лежат в библиотеке pyRevit
                Git($"clone https://github.com/pyrevitlabs/pyRevit.git --depth 1 --branch v4.8.16.24121+2117 {pyRevitOutput}");
                Git($"clone https://github.com/dosymep/BIM4Everyone.git --depth 1 --branch master {Bim4EveryoneOutput}");
            });

    Target Compile =>
        _ => _
            .DependsOn(Restore)
            .DependsOn(DownloadBim4Everyone)
            .Executes(() => {
                DotNetBuild(s => s
                    .EnableForce()
                    .DisableNoRestore()
                    .SetConfiguration(Configuration)
                    .CombineWith(
                        GetSrcProjectPaths(),
                        (settings, plugin) => settings
                            .SetProjectFile(plugin)
                            .CombineWith(
                                RevitVersions,
                                (settings, version) => settings
                                    .SetProperty("RevitVersion", version)
                                    .SetOutputDirectory(ArtifactPath / version.ToString()))));
            });

    Target Publish =>
        _ => _
            .DependsOn(Restore)
            .OnlyWhenStatic(() => IsLocalBuild)
            .Executes(() => {
                DotNetPublish(s => s
                    .EnableForce()
                    .DisableNoRestore()
                    .SetConfiguration(Configuration)
                    .CombineWith(
                        GetSrcProjectPaths(),
                        (settings, plugin) => settings
                            .SetProject(plugin)
                            .CombineWith(
                                RevitVersions,
                                (settings, version) => settings
                                    .SetProperty("RevitVersion", version)
                                    .SetOutput(PublishOutput / version.ToString()))));
            });

    Target Test =>
        _ => _
            .DependsOn(Restore)
            .OnlyWhenStatic(() => IsLocalBuild)
            .Executes(() => {
                DotNetRun(s => s
                    .DisableNoRestore()
                    .SetConfiguration(Configuration)
                    .CombineWith(
                        GetTestProjectPaths(),
                        (settings, plugin) => settings
                            .SetProjectFile(plugin)
                            .CombineWith(
                                RevitVersions,
                                (settings, version) => settings
                                    .SetProperty("RevitVersion", version)
                                    .SetApplicationArguments(
                                        "--results-directory", TestResultsPath,
                                        "--coverage", "--coverage-output", $"TestCoverage_{version}.coverage",
                                        "--report-trx", "--report-trx-filename", $"TestResult_{version}.trx"))));
            });

    Target DocsCompile =>
        _ => _
            .DependsOn(Compile)
            .Executes(() => {
                ProcessTasks.StartProcess(
                        "docfx",
                        DocsConfig
                        + (IsLocalBuild
                            ? " --serve"
                            : string.Empty),
                        RootDirectory)
                    .WaitForExit();
            });

    AbsolutePath[] GetSrcProjectPaths() {
        return [
            ..Solution.AllProjects
                .Where(item => item.Parent.ToString()?.Equals("src") == true)
        ];
    }

    AbsolutePath[] GetTestProjectPaths() {
        return [
            ..Solution.AllProjects
                .Where(item => item.Parent.ToString()?.Equals("tests") == true)
        ];
    }
}
