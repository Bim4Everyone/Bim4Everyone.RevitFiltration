using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class DoubleOperatorToken : IOperatorToken {
    public DoubleOperatorToken(IOperator @operator, double value, double epsilon) {
        Operator = @operator;
        Value = value;
        Epsilon = epsilon;
    }

    public double Value { get; }

    public double Epsilon { get; }

    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId) {
        return Operator.Create(paramId, Value, Epsilon);
    }
}
