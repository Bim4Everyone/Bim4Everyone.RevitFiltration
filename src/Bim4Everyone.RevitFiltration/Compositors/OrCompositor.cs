using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Compositors;

internal class OrCompositor : ICompositor {
    public ElementFilter Create(ICollection<ElementFilter> filters) {
        return new LogicalOrFilter(filters.ToArray());
    }
}
