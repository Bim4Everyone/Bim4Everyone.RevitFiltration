using System.Globalization;

using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Models.Utils;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;
using Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

using dosymep.Bim4Everyone;
using dosymep.Bim4Everyone.SystemParams;
using dosymep.Revit;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Params;

internal class ParamModel : IEquatable<ParamModel> {
    public ParamModel(RevitParam parameter) {
        if(parameter == null) {
            throw new ArgumentNullException(nameof(parameter));
        }

        Name = parameter.Name;
        if(string.IsNullOrWhiteSpace(parameter.Id)) {
            throw new ArgumentException($"{nameof(Id)} is null, param name: {Name}");
        }

        Id = parameter is SystemParam sysParam ? sysParam.SystemParamId.ToString() : parameter.Id;
        StorageType = parameter.StorageType;
        if(StorageType == StorageType.Double) {
            // UnitType нужен только для конвертации метрических единиц, которые вводит пользователь в имперские единицы ревита,
            // в этом случае StorageType всегда Double.
            // При этом необходимость в конвертации есть только для размеров: длины, площади, объемы и т.п.
#if REVIT2021_OR_GREATER
            try {
                UnitType = parameter.UnitType;
                TypeId = parameter.UnitType?.TypeId ?? string.Empty;
            } catch(ArgumentOutOfRangeException) {
                UnitType = ForgeTypeIdExtensions.EmptyForgeTypeId;
                TypeId = string.Empty;
            }
#else
            UnitType = parameter.UnitType;
#endif
        }
    }

#if REVIT2021_OR_GREATER
    [JsonConstructor]
    public ParamModel(string name, string id, string typeId, StorageType storageType) {
        Name = name;
        Id = id;
        TypeId = typeId;
        StorageType = storageType;
        UnitType = new ForgeTypeId(TypeId);
    }
#else
    [JsonConstructor]
    public ParamModel(string name, string id, UnitType unitType, StorageType storageType) {
        Name = name;
        Id = id;
        UnitType = unitType;
        StorageType = storageType;
    }
#endif

    [JsonProperty]
    public string Name { get; }

    [JsonProperty]
    public StorageType StorageType { get; }

    [JsonProperty]
    public string Id { get; }

#if REVIT2021_OR_GREATER
    [JsonIgnore]
    private ForgeTypeId UnitType { get; } = new();

    [JsonProperty]
    private string TypeId { get; } = string.Empty;
#else
    [JsonProperty]
    private UnitType UnitType { get; }
#endif

    public ICollection<OperatorKind> GetOperatorKinds() {
        if(Id == nameof(BuiltInParameter.ELEM_PARTITION_PARAM)) {
            // у рабочего набора StorageType - Integer, но поведение как у строкового параметра
            return OperatorKindUtils.GetOperatorKinds(StorageType.String);
        }

        return OperatorKindUtils.GetOperatorKinds(StorageType);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, IVisitor visitor, ParamValue paramValue) {
        paramValue.AddInnerRule(logicalFilter, visitor, this);
    }

    public ParamValue GetParamValueFromString(string displayValue) {
        if(string.IsNullOrWhiteSpace(displayValue)) {
            throw new ArgumentException(nameof(displayValue));
        }

        if(Id == nameof(BuiltInParameter.ELEM_PARTITION_PARAM)) {
            // для параметра "Рабочий набор" StorageType - Integer, но значение используется строковое
            return new StringParamValue(displayValue, displayValue);
        }

        if(StorageType == StorageType.Double) {
            if(DoubleValueParser.TryParse(displayValue, UnitType, out double res)) {
                return ParamValue.GetParamValue(StorageType, res.ToString(CultureInfo.InvariantCulture), displayValue);
            }
        }

        return ParamValue.GetParamValue(StorageType, displayValue, displayValue);
    }

    /// <summary>
    ///     Проверяет, является ли параметр системным
    /// </summary>
    /// <param name="builtInParameter">Значение системного параметра, если таковым является текущий параметр</param>
    /// <returns>True, если параметр является системным, иначе false</returns>
    public bool IsSystemParam(out BuiltInParameter builtInParameter) {
        return Enum.TryParse(Id, out builtInParameter);
    }

    public bool Equals(ParamModel? other) {
        if(other is null) {
            return false;
        }

        if(ReferenceEquals(this, other)) {
            return true;
        }

        return Name == other.Name
               && StorageType == other.StorageType
               && Id.Equals(other.Id);
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

        return Equals((ParamModel) obj);
    }

    public override int GetHashCode() {
        unchecked {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ (int) StorageType;
            hashCode = (hashCode * 397) ^ Id.GetHashCode();
            return hashCode;
        }
    }
}
