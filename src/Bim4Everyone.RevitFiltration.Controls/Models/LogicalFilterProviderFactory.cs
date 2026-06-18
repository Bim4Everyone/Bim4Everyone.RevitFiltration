namespace Bim4Everyone.RevitFiltration.Controls.Models;

internal class LogicalFilterProviderFactory : ILogicalFilterProviderFactory {
    private readonly ILogicalFilterFactory _factory;

    public LogicalFilterProviderFactory(ILogicalFilterFactory factory) {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    [Obsolete("Используйте перегрузку Create(DataProvider).")]
    public ILogicalFilterProvider Create(IDataProvider dataProvider) {
        if(dataProvider == null) {
            throw new ArgumentNullException(nameof(dataProvider));
        }

        return Create(Adapt(dataProvider));
    }

    [Obsolete("Используйте перегрузку Create(DataProvider, ILogicalFilterContext).")]
    public ILogicalFilterProvider Create(
        IDataProvider dataProvider,
        ILogicalFilterContext contextToLoad) {
        if(dataProvider == null) {
            throw new ArgumentNullException(nameof(dataProvider));
        }
        
        if(contextToLoad == null) {
            throw new ArgumentNullException(nameof(contextToLoad));
        }

        return Create(Adapt(dataProvider), contextToLoad);
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

    [Obsolete("Адаптер для обратной совместимости с IDataProvider.")]
    private DataProvider Adapt(IDataProvider dataProvider) {
        return new DataProvider(
            dataProvider.GetCategories(),
            dataProvider.GetParams,
            dataProvider.GetDocuments());
    }
}
