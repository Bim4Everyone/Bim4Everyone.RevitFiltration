using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class StringOperatorToken : IOperatorToken {
    [JsonConstructor]
    public StringOperatorToken(IOperator @operator, string value) {
        Operator = @operator;
        Value = value;
    }

    [JsonProperty]
    public string Value { get; }

    [JsonProperty]
    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId, IOptions options) {
        return Operator.Create(paramId, Value);
    }
}
