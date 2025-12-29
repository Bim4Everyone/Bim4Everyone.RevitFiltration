using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class CompositorViewModel : BaseViewModel, IEquatable<CompositorViewModel> {
    public CompositorViewModel(CompositorKind compositor) {
        CompositorKind = compositor;
        Name = CompositorKind.ToString(); // TODO
    }

    public bool IsAndCompositor => CompositorKind == CompositorKind.And;

    public CompositorKind CompositorKind { get; }

    public string Name { get; }

    public bool Equals(CompositorViewModel? other) {
        if(other is null) {
            return false;
        }

        if(ReferenceEquals(this, other)) {
            return true;
        }

        return CompositorKind == other.CompositorKind;
    }

    public override bool Equals(object? obj) {
        if(obj is null) {
            return false;
        }

        if(ReferenceEquals(this, obj)) {
            return true;
        }

        if(obj.GetType() != GetType()) {
            return false;
        }

        return Equals((CompositorViewModel) obj);
    }

    public override int GetHashCode() {
        return (int) CompositorKind;
    }
}
