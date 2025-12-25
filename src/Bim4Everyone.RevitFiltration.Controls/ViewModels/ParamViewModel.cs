using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.Params;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class ParamViewModel : BaseViewModel, IEquatable<ParamViewModel> {
    public ParamViewModel(ParamModel paramModel) {
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
        return ParamModel.StorageType switch {
            StorageType.Integer => int.TryParse(strValue, out _)
                ? string.Empty
                : $"Значение параметра {Name} должно быть целым числом",
            StorageType.Double => double.TryParse(strValue, out _)
                ? string.Empty
                : $"Значение параметра {Name} должно быть числом",
            StorageType.String => string.Empty,
            StorageType.ElementId => string.Empty,
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
