using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Filtration;

/// <summary>
///     Правило фильтрации на основе параметра.
/// </summary>
internal class Rule : IFilterRule {
    /// <summary>
    ///     Создание правила на основе параметра и настроек фильтрации (условия и проверяемого значения).
    /// </summary>
    /// <param name="param">Параметр для создания фильтра.</param>
    /// <param name="operatorToken">Настройки, определяющие условие (больше, меньше и т.д.) и значение фильтра.</param>
    [JsonConstructor]
    public Rule(IParam param, IOperatorToken operatorToken) {
        Param = param;
        OperatorToken = operatorToken;
    }

    /// <inheritdoc />
    [JsonProperty]
    public IParam Param { get; }

    /// <inheritdoc />
    [JsonProperty]
    public IOperatorToken OperatorToken { get; }

    /// <inheritdoc />
    public FilterRule CreateFilterRule(Document document, Options options) {
        return OperatorToken.Create(Param.GetId(document), document, options);
    }
}
