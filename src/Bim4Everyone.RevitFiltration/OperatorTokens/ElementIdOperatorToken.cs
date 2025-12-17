using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class ElementIdOperatorToken : IOperatorToken {
    public ElementIdOperatorToken(IOperator @operator, ElementId value) {
        Operator = @operator;
        Value = value;
    }

    public ElementId Value { get; }

    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId) {
        return Operator.Create(paramId, Value);
    }
}
