using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class StringOperatorToken : IOperatorToken {
    public StringOperatorToken(IOperator @operator, string value) {
        Operator = @operator;
        Value = value;
    }

    public string Value { get; }

    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId) {
        return Operator.Create(paramId, Value);
    }
}
