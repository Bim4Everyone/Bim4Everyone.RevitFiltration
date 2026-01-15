using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

/// <summary>
///     Настройки для проверки строкового параметра.
/// </summary>
internal class StringOperatorToken : IOperatorToken {
    /// <summary>
    ///     Создание токена для проверки строкового параметра.
    /// </summary>
    /// <param name="operator">
    ///     Оператор, определяющий условие фильтрации (больше, меньше, равно, содержит и т.д.)
    /// </param>
    /// <param name="value">Значение для проверки в фильтре.</param>
    [JsonConstructor]
    public StringOperatorToken(IOperator @operator, string value) {
        Operator = @operator;
        Value = value;
    }

    /// <summary>
    ///     Значение для проверки в фильтре.
    /// </summary>
    [JsonProperty]
    public string Value { get; }

    /// <inheritdoc />
    [JsonProperty]
    public IOperator Operator { get; }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, IOptions options) {
        return Operator.Create(paramId, Value);
    }
}
