using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class DoubleOperatorToken : IOperatorToken {
    [JsonConstructor]
    public DoubleOperatorToken(IOperator @operator, double value) {
        Operator = @operator;
        Value = value;
    }

    [JsonProperty]
    public double Value { get; }

    [JsonProperty]
    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId, IOptions options) {
        return Operator.Create(paramId, Value, options.Tolerance);
    }
}
