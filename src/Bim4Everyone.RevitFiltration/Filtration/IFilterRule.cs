using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

namespace Bim4Everyone.RevitFiltration.Filtration;

/// <summary>
///     Отвечает за создание правила по фильтрации на основе параметра, например "Высота больше 5000".
/// </summary>
internal interface IFilterRule {
    /// <summary>
    ///     Параметр, который проверяется при фильтрации.
    /// </summary>
    internal IParam Param { get; }

    /// <summary>
    ///     Настройки для определения условия фильтрации и проверяемого значения.
    /// </summary>
    internal IOperatorToken OperatorToken { get; }

    /// <summary>
    ///     Создаёт правило фильтрации на основе документа Revit.
    /// </summary>
    /// <param name="document">Документ Revit.</param>
    /// <param name="options">Настройки генерации фильтра.</param>
    /// <returns>Возвращает объект FilterRule из Revit.</returns>
    internal FilterRule CreateFilterRule(Document document, Options options);
}
