using Bim4Everyone.RevitFiltration.Compositors;

namespace Bim4Everyone.RevitFiltration.Filtration;

internal class LogicalFilterFactory : ILogicalFilterFactory {
    public ILogicalFilter CreateAndFilter() {
        return new Set(new AndCompositor());
    }

    public ILogicalFilter CreateOrFilter() {
        return new Set(new OrCompositor());
    }
}
