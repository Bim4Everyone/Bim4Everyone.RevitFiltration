using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models;

internal class LogicalFilterProviderFactory : ILogicalFilterProviderFactory {
    private readonly ILogicalFilterFactory _factory;

    public LogicalFilterProviderFactory(ILogicalFilterFactory factory) {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public ILogicalFilterProvider Create(DataProvider dataProvider) {
        if(dataProvider == null) {
            throw new ArgumentNullException(nameof(dataProvider));
        }

        return new LogicalFilterProvider(dataProvider, _factory);
    }

    public ILogicalFilterProvider Create(
        DataProvider dataProvider,
        ILogicalFilterContext contextToLoad) {
        if(dataProvider == null) {
            throw new ArgumentNullException(nameof(dataProvider));
        }

        if(contextToLoad == null) {
            throw new ArgumentNullException(nameof(contextToLoad));
        }

        return new LogicalFilterProvider(dataProvider, _factory, contextToLoad);
    }

    public ILogicalFilterProvider Create(
        DataProvider dataProvider,
        ILogicalFilter filterToLoad,
        ICollection<BuiltInCategory> categories) {
        if(dataProvider == null) {
            throw new ArgumentNullException(nameof(dataProvider));
        }

        if(filterToLoad == null) {
            throw new ArgumentNullException(nameof(filterToLoad));
        }

        if(categories == null) {
            throw new ArgumentNullException(nameof(categories));
        }

        return new LogicalFilterProvider(dataProvider, _factory, filterToLoad, categories);
    }
}
