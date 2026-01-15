using Bim4Everyone.RevitFiltration.Compositors;

namespace Bim4Everyone.RevitFiltration.Filtration;

/// <inheritdoc />
internal class LogicalFilterFactory : ILogicalFilterFactory {
    /// <inheritdoc />
    public ILogicalFilter CreateAndFilter() {
        return new LogicalFilter(new AndCompositor());
    }

    /// <inheritdoc />
    public ILogicalFilter CreateOrFilter() {
        return new LogicalFilter(new OrCompositor());
    }
}
