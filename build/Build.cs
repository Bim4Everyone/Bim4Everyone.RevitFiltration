using System;
using System.IO;
using System.Linq;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Components;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild, IHazSolution {
    [Parameter]
    readonly AbsolutePath ArtifactPath;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter]
    readonly AbsolutePath DocsCaches = RootDirectory / Path.Combine("docs", "api");

    [Parameter]
    readonly string DocsConfig = Path.Combine("docs", "docfx.json");

    [Parameter]
    readonly string DocsOutput = Path.Combine("docs", "_site");

    /// <summary>
    ///     Max Revit version.
    /// </summary>
    [Parameter("Max Revit version.")]
    readonly int MaxVersion = 2024;

    /// <summary>
    ///     Min Revit version.
    /// </summary>
    [Parameter("Min Revit version.")]
    readonly int MinVersion = 2020;

    [Parameter]
    readonly AbsolutePath PublishOutput;

    [Parameter("Build Revit versions.")]
    readonly int[] RevitVersions = [2020, 2021, 2022, 2023, 2024];

    public Build() {
        ArtifactPath = RootDirectory / "bin";
        AbsolutePath appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        PublishOutput = appdataFolder / "pyRevit" / "Extensions" / "BIM4Everyone.lib" / "dosymep_libs" / "libs";
    }

    AbsolutePath RevitFiltrationProject =>
        RootDirectory / "src" / "Bim4Everyone.RevitFiltration" / "Bim4Everyone.RevitFiltration.csproj";

    AbsolutePath RevitFiltrationControlsProject =>
        RootDirectory
        / "src"
        / "Bim4Everyone.RevitFiltration.Controls"
        / "Bim4Everyone.RevitFiltration.Controls.csproj";

    AbsolutePath RevitFiltrationNinjectProject =>
        RootDirectory
        / "src"
        / "Bim4Everyone.RevitFiltration.Ninject"
        / "Bim4Everyone.RevitFiltration.Ninject.csproj";

    Target Clean =>
        _ => _
            .Before(Restore)
            .Executes(() => {
                ArtifactPath.CreateOrCleanDirectory();
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
                foreach(var project in GetSrcProjectPaths()) {
                    DotNetRestore(s => s.SetProjectFile(project));
                }
            });

    Target Compile =>
        _ => _
            .DependsOn(Restore)
            .Executes(() => {
                foreach(var project in GetSrcProjectPaths()) {
                    DotNetBuild(s => s
                        .EnableForce()
                        .DisableNoRestore()
                        .SetConfiguration(Configuration)
                        .SetProjectFile(project)
                        .CombineWith(
                            RevitVersions,
                            (settings, version) => {
                                return settings
                                    .SetOutputDirectory(ArtifactPath / version.ToString())
                                    .SetProperty("RevitVersion", version);
                            }));
                }
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

    Target Publish =>
        _ => _
            .DependsOn(Restore)
            .OnlyWhenStatic(() => IsLocalBuild)
            .Executes(() => {
                var projects = new[] {
                    RevitFiltrationProject, RevitFiltrationControlsProject, RevitFiltrationNinjectProject
                };
                foreach(var project in projects) {
                    DotNetBuild(s => s
                        .EnableForce()
                        .DisableNoRestore()
                        .SetConfiguration(Configuration)
                        .SetProjectFile(project)
                        .CombineWith(
                            RevitVersions,
                            (settings, version) => {
                                return settings
                                    .SetOutputDirectory(PublishOutput / version.ToString())
                                    .SetProperty("RevitVersion", version);
                            }));
                }
            });

    AbsolutePath[] GetSrcProjectPaths() => [
        RevitFiltrationProject, RevitFiltrationControlsProject, RevitFiltrationNinjectProject
    ];

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);
}
