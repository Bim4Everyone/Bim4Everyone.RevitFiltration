using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class EmptyOperatorToken : IOperatorToken {
    [JsonConstructor]
    public EmptyOperatorToken(IOperator @operator) {
        Operator = @operator;
    }

    [JsonProperty]
    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId) {
        return Operator.Create(paramId);
    }
}
