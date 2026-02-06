using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Compositors;

/// <summary>
///     Создает фильтр с логическим правилом "ИЛИ".
/// </summary>
internal class OrCompositor : ICompositor {
    public ElementFilter Create(ICollection<ElementFilter> filters) {
        return new LogicalOrFilter(filters.ToArray());
    }
}
