namespace Bim4Everyone.RevitFiltration.Controls.Models.Value;

internal abstract class ParamValue<T> : ParamValue, IComparable<ParamValue<T>> where T : IComparable {
    protected ParamValue(T value, string displayValue)
        : base(displayValue) {
        TValue = value;
    }

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

    public override int CompareTo(ParamValue? other) {
        return CompareTo(other as ParamValue<T>);
    }
}
