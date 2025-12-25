using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class ParamValueViewModel : BaseViewModel, IEquatable<ParamValueViewModel> {
    public ParamValueViewModel(ParamValue paramValue) {
        ParamValue = paramValue ?? throw new ArgumentNullException(nameof(paramValue));
    }

    public string DisplayValue => ParamValue.DisplayValue;

    public ParamValue ParamValue { get; }

    public bool Equals(ParamValueViewModel? other) {
        if(other is null) {
            return false;
        }

        if(ReferenceEquals(this, other)) {
            return true;
        }

        return ParamValue.Equals(other.ParamValue);
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

        return Equals((ParamValueViewModel) obj);
    }

    public override int GetHashCode() {
        return ParamValue.GetHashCode();
    }
}
