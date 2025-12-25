using Bim4Everyone.RevitFiltration.Controls.Core;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class CompositorViewModel : BaseViewModel {
    public CompositorViewModel(CompositorKind compositor) {
        CompositorKind = compositor;
        Name = CompositorKind.ToString(); // TODO
    }

    public CompositorKind CompositorKind { get; }

    public string Name { get; }
}
