using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

/// <summary>
///     Настройки для проверки параметра с числом с плавающей точкой.
/// </summary>
internal class DoubleOperatorToken : IOperatorToken {
    /// <summary>
    ///     Создание токена для проверки параметра с числом с плавающей точкой.
    /// </summary>
    /// <param name="operator">Оператор, определяющий условие фильтрации (больше, меньше, равно и т.д.).</param>
    /// <param name="value">Значение для проверки в фильтре.</param>
    [JsonConstructor]
    public DoubleOperatorToken(IOperator @operator, double value) {
        Operator = @operator;
        Value = value;
    }

    /// <summary>
    ///     Значение для проверки в фильтре.
    /// </summary>
    [JsonProperty]
    public double Value { get; }

    /// <inheritdoc />
    [JsonProperty]
    public IOperator Operator { get; }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, Document document, Options options) {
        return Operator.Create(paramId, Value, options.Tolerance);
    }
}
