using Bim4Everyone.RevitFiltration.Compositors;

namespace Bim4Everyone.RevitFiltration.Filtration;

internal class LogicalFilterFactory : ILogicalFilterFactory {
    public ILogicalFilter CreateAndFilter(IOptions options) {
        if(options == null) {
            throw new ArgumentNullException(nameof(options));
        }

        return new Set(new AndCompositor(), options);
    }

    public ILogicalFilter CreateOrFilter(IOptions options) {
        if(options == null) {
            throw new ArgumentNullException(nameof(options));
        }

        return new Set(new OrCompositor(), options);
    }
}
