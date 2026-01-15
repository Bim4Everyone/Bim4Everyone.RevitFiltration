using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Compositors;

/// <summary>
///     Создает фильтр с логическим правилом "И" или "ИЛИ".
/// </summary>
internal interface ICompositor {
    internal ElementFilter Create(ICollection<ElementFilter> filters);
}
