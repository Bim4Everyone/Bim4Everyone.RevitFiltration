using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Compositors;

/// <summary>
///     Создает фильтр с логическим правилом "И".
/// </summary>
internal class AndCompositor : ICompositor {
    public ElementFilter Create(ICollection<ElementFilter> filters) {
        return new LogicalAndFilter(filters.ToArray());
    }
}
