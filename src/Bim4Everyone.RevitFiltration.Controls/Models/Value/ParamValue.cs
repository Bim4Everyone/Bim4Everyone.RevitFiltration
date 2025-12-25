using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Value;

internal abstract class ParamValue : IComparable<ParamValue>, IEquatable<ParamValue> {
    protected ParamValue(string displayValue) {
        DisplayValue = displayValue ?? throw new ArgumentNullException(nameof(displayValue));
    }

    [JsonIgnore]
    public abstract object Value { get; }

    [JsonProperty]
    public string DisplayValue { get; }

    public virtual int CompareTo(ParamValue? other) {
        if(ReferenceEquals(this, other)) {
            return 0;
        }

        if(other is null) {
            return 1;
        }

        return Comparer<object>.Default.Compare(Value, other.Value);
    }

    public virtual bool Equals(ParamValue? other) {
        return other != null
               && EqualityComparer<string>.Default.Equals(DisplayValue, other.DisplayValue);
    }

    public abstract void AddInnerRule(ILogicalFilter logicalFilter, IVisitor visitor, ParamModel paramModel);

    public static ParamValue GetParamValue(ParamModel paramModel, string value, string displayValue) {
        if(paramModel == null) {
            throw new ArgumentNullException(nameof(paramModel));
        }

        if(value == null) {
            throw new ArgumentNullException(nameof(value));
        }

        if(displayValue == null) {
            throw new ArgumentNullException(nameof(displayValue));
        }

        return paramModel.StorageType switch {
            StorageType.Integer => new IntParamValue(int.Parse(value), displayValue),
            StorageType.String => new StringParamValue(value, displayValue),
            StorageType.Double => new DoubleParamValue(double.Parse(value), displayValue),
            StorageType.ElementId => new ElementIdParamValue(value, displayValue),
            _ => throw new InvalidOperationException()
        };
    }

    public override bool Equals(object? obj) {
        return Equals(obj as ParamValue);
    }

    public override int GetHashCode() {
        return -1937169414 + EqualityComparer<string>.Default.GetHashCode(DisplayValue);
    }
}
