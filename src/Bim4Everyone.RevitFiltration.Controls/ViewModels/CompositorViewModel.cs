using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class CompositorViewModel : BaseViewModel {
    public CompositorViewModel(CompositorKind compositor) {
        CompositorKind = compositor;
        Name = CompositorKind.ToString(); // TODO
    }

    public bool IsAndCompositor => CompositorKind == CompositorKind.And;

    public CompositorKind CompositorKind { get; }

    public string Name { get; }
}
