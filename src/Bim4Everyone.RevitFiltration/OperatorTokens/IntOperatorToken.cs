using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class IntOperatorToken : IOperatorToken {
    public IntOperatorToken(IOperator @operator, int value) {
        Operator = @operator;
        Value = value;
    }

    public int Value { get; }

    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId) {
        return Operator.Create(paramId, Value);
    }
}
