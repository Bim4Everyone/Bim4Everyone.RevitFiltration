using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Services;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class OperatorViewModel : BaseViewModel, IEquatable<OperatorViewModel> {
    private readonly ILocalizationProvider _localization;

    public OperatorViewModel(ILocalizationProvider localization, OperatorKind @operator) {
        _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        Operator = @operator;
        Name = _localization.GetLocalizedString($"{nameof(OperatorKind)}.{@operator}");
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
