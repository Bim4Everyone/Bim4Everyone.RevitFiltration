using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Services;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class ParamViewModel : BaseViewModel, IEquatable<ParamViewModel> {
    private readonly ILocalizationProvider _localization;

    public ParamViewModel(ILocalizationProvider localization, ParamModel paramModel) {
        _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        ParamModel = paramModel ?? throw new ArgumentNullException(nameof(paramModel));
    }

    public ParamModel ParamModel { get; }

    public string Name => ParamModel.Name;

    public bool Equals(ParamViewModel? other) {
        if(other is null) {
            return false;
        }

        if(ReferenceEquals(this, other)) {
            return true;
        }

        return ParamModel.Equals(other.ParamModel);
    }

    public string GetErrorText(string strValue) {
        if(ParamModel.Id == nameof(BuiltInParameter.ELEM_PARTITION_PARAM)) {
            // для параметра "Рабочий набор" StorageType - Integer, но значение используется строковое
            return string.IsNullOrWhiteSpace(strValue)
                ? _localization.GetLocalizedString("B4E.Filtration.Validation.ParamValueNotSet", Name)
                : string.Empty;
        }

        return ParamModel.StorageType switch {
            StorageType.Integer => int.TryParse(strValue, out _)
                ? string.Empty
                : _localization.GetLocalizedString("B4E.Filtration.Validation.ParamValueMustBeInt", Name),
            StorageType.Double => double.TryParse(strValue, out _)
                ? string.Empty
                : _localization.GetLocalizedString("B4E.Filtration.Validation.ParamValueMustBeDouble", Name),
            StorageType.String => string.IsNullOrWhiteSpace(strValue)
                ? _localization.GetLocalizedString("B4E.Filtration.Validation.ParamValueNotSet", Name)
                : string.Empty,
            StorageType.ElementId => string.IsNullOrWhiteSpace(strValue)
                ? _localization.GetLocalizedString("B4E.Filtration.Validation.ParamValueNotSet", Name)
                : string.Empty,
            _ => throw new NotSupportedException()
        };
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

        return Equals((ParamViewModel) obj);
    }

    public override int GetHashCode() {
        return ParamModel.GetHashCode();
    }
}
