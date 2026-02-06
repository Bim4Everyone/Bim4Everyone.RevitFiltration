using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

/// <summary>
///     Настройки для проверки параметра с типом ElementId.
/// </summary>
internal class ElementIdOperatorToken : IOperatorToken {
    /// <summary>
    ///     Создание токена для проверки параметра с типом ElementId.
    /// </summary>
    /// <param name="operator">Оператор, определяющий условие фильтрации (больше, меньше, равно и т.д.).</param>
    /// <param name="value">Значение для проверки в фильтре.</param>
    [JsonConstructor]
    public ElementIdOperatorToken(IOperator @operator, ElementId value) {
        Operator = @operator;
        Value = value;
    }

    /// <summary>
    ///     Значение для проверки в фильтре.
    /// </summary>
    [JsonProperty]
    public ElementId Value { get; }

    /// <inheritdoc />
    [JsonProperty]
    public IOperator Operator { get; }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, Document document, IOptions options) {
        return Operator.Create(paramId, Value);
    }
}
