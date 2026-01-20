using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

/// <summary>
///     Настройки для проверки параметра на наличие значения.
/// </summary>
internal class EmptyOperatorToken : IOperatorToken {
    /// <summary>
    ///     Создание токена для проверки параметра на наличие значения.
    /// </summary>
    /// <param name="operator">Оператор, определяющий условие фильтрации (без значения, имеет значение).</param>
    [JsonConstructor]
    public EmptyOperatorToken(IOperator @operator) {
        Operator = @operator;
    }

    /// <inheritdoc />
    [JsonProperty]
    public IOperator Operator { get; }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, Document document, IOptions options) {
        return Operator.Create(paramId);
    }
}
