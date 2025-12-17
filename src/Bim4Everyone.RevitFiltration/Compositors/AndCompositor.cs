using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Compositors;

internal class AndCompositor : ICompositor {
    public ElementFilter Create(ICollection<ElementFilter> filters) {
        return new LogicalAndFilter(filters.ToArray());
    }
}
