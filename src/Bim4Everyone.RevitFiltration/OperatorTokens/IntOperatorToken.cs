using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class IntOperatorToken : IOperatorToken {
    [JsonConstructor]
    public IntOperatorToken(IOperator @operator, int value) {
        Operator = @operator;
        Value = value;
    }

    [JsonProperty]
    public int Value { get; }

    [JsonProperty]
    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId, IOptions options) {
        return Operator.Create(paramId, Value);
    }
}
