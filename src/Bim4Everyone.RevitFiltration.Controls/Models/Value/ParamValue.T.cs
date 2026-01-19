using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Value;

internal abstract class ParamValue<T> : ParamValue, IEquatable<ParamValue<T>>, IComparable<ParamValue<T>>
    where T : IComparable {
    protected ParamValue(T tValue, string displayValue)
        : base(displayValue) {
        TValue = tValue ?? throw new ArgumentNullException(nameof(tValue));
    }

    [JsonProperty]
    public T TValue { get; }

    public override object Value => TValue;

    public int CompareTo(ParamValue<T>? other) {
        if(ReferenceEquals(this, other)) {
            return 0;
        }

        if(other is null) {
            return 1;
        }

        return TValue.CompareTo(other.TValue);
    }

    public bool Equals(ParamValue<T>? other) {
        if(other is null) {
            return false;
        }

        if(ReferenceEquals(this, other)) {
            return true;
        }

        return EqualityComparer<T>.Default.Equals(TValue, other.TValue);
    }

    public override int CompareTo(ParamValue? other) {
        return CompareTo(other as ParamValue<T>);
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

        return Equals((ParamValue<T>) obj);
    }

    public override int GetHashCode() {
        unchecked {
            return (base.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(TValue);
        }
    }
}
