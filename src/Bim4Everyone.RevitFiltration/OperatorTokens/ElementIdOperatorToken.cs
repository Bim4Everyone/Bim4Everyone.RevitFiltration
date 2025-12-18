using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class ElementIdOperatorToken : IOperatorToken {
    [JsonConstructor]
    public ElementIdOperatorToken(IOperator @operator, ElementId value) {
        Operator = @operator;
        Value = value;
    }

    [JsonProperty]
    public ElementId Value { get; }

    [JsonProperty]
    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId, IOptions options) {
        return Operator.Create(paramId, Value);
    }
}
