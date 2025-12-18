using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

internal class DoubleOperatorToken : IOperatorToken {
    [JsonConstructor]
    public DoubleOperatorToken(IOperator @operator, double value, double epsilon) {
        Operator = @operator;
        Value = value;
        Epsilon = epsilon;
    }

    [JsonProperty]
    public double Value { get; }

    [JsonProperty]
    public double Epsilon { get; }

    [JsonProperty]
    public IOperator Operator { get; }

    public FilterRule Create(ElementId paramId) {
        return Operator.Create(paramId, Value, Epsilon);
    }
}
