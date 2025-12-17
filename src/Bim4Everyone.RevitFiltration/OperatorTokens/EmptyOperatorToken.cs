using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class EmptyOperatorToken : IOperatorToken {
    public EmptyOperatorToken(IOperator @operator) {
        Operator = @operator;
    }

    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId) {
        return Operator.Create(paramId);
    }
}
