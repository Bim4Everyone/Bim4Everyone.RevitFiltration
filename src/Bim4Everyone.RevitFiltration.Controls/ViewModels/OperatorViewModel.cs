using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class OperatorViewModel : BaseViewModel, IEquatable<OperatorViewModel> {
    public OperatorViewModel(OperatorKind @operator) {
        Operator = @operator;
        Name = @operator.ToString(); // TODO
    }

    public OperatorKind Operator { get; }

    public string Name { get; }

    public bool Equals(OperatorViewModel? other) {
        if(other is null) {
            return false;
        }

        if(ReferenceEquals(this, other)) {
            return true;
        }

        return Operator == other.Operator;
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

        return Equals((OperatorViewModel) obj);
    }

    public override int GetHashCode() {
        return (int) Operator;
    }
}
