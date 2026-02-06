using System;
using System.Collections.Generic;
using System.Linq;

using dosymep.Nuke.RevitVersions;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Components;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild, IHazSolution {
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    /// <summary>
    ///     Max Revit version.
    /// </summary>
    [Parameter("Max Revit version.")]
    readonly RevitVersion MaxVersion = RevitVersion.Rv2024;

    /// <summary>
    ///     Min Revit version.
    /// </summary>
    [Parameter("Min Revit version.")]
    readonly RevitVersion MinVersion = RevitVersion.Rv2020;

    [Parameter]
    readonly AbsolutePath Output = RootDirectory / "bin";

    readonly AbsolutePath PublishOutput;

    [Parameter("Build Revit versions.")]
    readonly RevitVersion[] RevitVersions = new RevitVersion[0];

    IEnumerable<RevitVersion> BuildRevitVersions;

    public Build() {
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
                Output.CreateOrCleanDirectory();
                RootDirectory.GlobDirectories("**/bin", "**/obj")
                    .Where(item => item != RootDirectory / "build" / "bin")
                    .Where(item => item != RootDirectory / "build" / "obj")
                    .DeleteDirectories();
            });

    Target Restore =>
        _ => _
            .DependsOn(Clean)
            .Executes(() => {
                var projects = new[] {
                    RevitFiltrationProject, RevitFiltrationControlsProject, RevitFiltrationNinjectProject
                };
                foreach(var project in projects) {
                    DotNetRestore(s => s.SetProjectFile(project));
                }
            });

    Target Compile =>
        _ => _
            .DependsOn(Restore)
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
                            BuildRevitVersions,
                            (settings, version) => {
                                return settings
                                    .SetOutputDirectory(Output / version)
                                    .SetProperty("RevitVersion", (int) version);
                            }));
                }
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
                            BuildRevitVersions,
                            (settings, version) => {
                                return settings
                                    .SetOutputDirectory(PublishOutput / version)
                                    .SetProperty("RevitVersion", (int) version);
                            }));
                }
            });

    protected override void OnBuildInitialized() {
        base.OnBuildInitialized();
        BuildRevitVersions = RevitVersions.Length > 0
            ? RevitVersions
            : RevitVersion.GetRevitVersions(MinVersion, MaxVersion);
    }

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);
}
