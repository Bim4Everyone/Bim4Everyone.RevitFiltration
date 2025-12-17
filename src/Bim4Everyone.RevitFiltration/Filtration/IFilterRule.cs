using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

namespace Bim4Everyone.RevitFiltration.Filtration;

/// <summary>
///     Отвечает за создание правила по фильтрации параметра, например "Высота больше 5000".
/// </summary>
internal interface IFilterRule {
    internal IParam Param { get; }
    internal IOperatorToken OperatorToken { get; }
    internal FilterRule CreateFilterRule(Document document);
}
