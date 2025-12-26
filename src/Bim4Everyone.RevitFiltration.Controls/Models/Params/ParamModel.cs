using System.Globalization;

using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Models.Utils;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;
using Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

using dosymep.Revit;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Params;

internal class ParamModel : IEquatable<ParamModel> {
    public ParamModel(IParam parameter) {
        if(parameter == null) {
            throw new ArgumentNullException(nameof(parameter));
        }

        Name = parameter.Name;
        Id = parameter.Id ?? throw new ArgumentException($"{nameof(IParam.Id)} is null");
#if REVIT_2020_OR_LESS
        UnitType = parameter.UnitType;
#else
        UnitTypeName = parameter.UnitType?.GetSpecTypeIdName()
                       ?? throw new ArgumentException($"{nameof(IParam.UnitType)} is null");
#endif
        StorageType = parameter.StorageType;
    }

#if REVIT_2020_OR_LESS
    [JsonConstructor]
    public ParamModel(string name, ElementId id, UnitType unitType, StorageType storageType) {
        Name = name;
        Id = id;
        UnitType = unitType;
        StorageType = storageType;
    }
#else
    [JsonConstructor]
    public ParamModel(string name, ElementId id, string unitTypeName, StorageType storageType) {
        Name = name;
        Id = id;
        UnitTypeName = unitTypeName;
        StorageType = storageType;
    }
#endif

    [JsonProperty]
    public string Name { get; }

#if REVIT_2020_OR_LESS
    [JsonProperty]
    public UnitType UnitType { get; }
#else
    [JsonIgnore]
    public ForgeTypeId UnitType => ForgeTypeIdExtensions.GetSpecIdByName(UnitTypeName);

    [JsonProperty]
    public string UnitTypeName { get; }
#endif

    [JsonProperty]
    public StorageType StorageType { get; }

    [JsonProperty]
    public ElementId Id { get; }

    public ICollection<OperatorKind> GetOperatorKinds() {
        return OperatorKindUtils.GetOperatorKinds(StorageType);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, IVisitor visitor, ParamValue paramValue) {
        paramValue.AddInnerRule(logicalFilter, visitor, this);
    }

    public ParamValue GetParamValueFromString(string displayValue) {
        if(string.IsNullOrWhiteSpace(displayValue)) {
            throw new ArgumentException(nameof(displayValue));
        }

        if(Id == new ElementId(BuiltInParameter.ELEM_PARTITION_PARAM)) {
            // для параметра "Рабочий набор" StorageType - Integer, но значение используется строковое
            return new StringParamValue(displayValue, displayValue);
        }

        if(StorageType == StorageType.Double) {
            if(DoubleValueParser.TryParse(displayValue, UnitType, out double res)) {
                return ParamValue.GetParamValue(this, res.ToString(CultureInfo.InvariantCulture), displayValue);
            }
        }

        return ParamValue.GetParamValue(this, displayValue, displayValue);
    }

    public bool Equals(ParamModel? other) {
        if(other is null) {
            return false;
        }

        if(ReferenceEquals(this, other)) {
            return true;
        }

        return Name == other.Name
               && UnitType.Equals(other.UnitType)
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
            hashCode = (hashCode * 397) ^ UnitType.GetHashCode();
            hashCode = (hashCode * 397) ^ (int) StorageType;
            hashCode = (hashCode * 397) ^ Id.GetHashCode();
            return hashCode;
        }
    }
}
