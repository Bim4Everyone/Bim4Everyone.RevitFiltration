using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Отвечает за создание правила по фильтрации параметра, например "Высота больше 5000".
/// </summary>
internal interface IFilterRule {
    internal IParam Param { get; }
    internal IOperatorToken OperatorToken { get; }
    internal FilterRule CreateFilterRule(Document document);
}
