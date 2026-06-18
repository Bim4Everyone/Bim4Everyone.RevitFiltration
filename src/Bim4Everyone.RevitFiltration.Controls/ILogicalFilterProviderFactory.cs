namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Фабрика для создания провайдеров фильтров по категориям.
/// </summary>
public interface ILogicalFilterProviderFactory {
    /// <summary>
    ///     Создает провайдер фильтра элементов по доступным данным.
    /// </summary>
    /// <param name="dataProvider">Провайдер доступных данных для выбора пользователя.</param>
    /// <returns>Провайдер контекста фильтра по категориям.</returns>
    [Obsolete("Используйте перегрузку Create(DataProvider).")]
    ILogicalFilterProvider Create(IDataProvider dataProvider);

    /// <summary>
    ///     Загружает заданный контекст и создает провайдер фильтра элементов по доступным данным.
    /// </summary>
    /// <param name="dataProvider">Провайдер доступных данных для выбора пользователя.</param>
    /// <param name="contextToLoad">
    ///     Контекст для загрузки. Категории и правила фильтрации по параметрам, которых нет в доступных, будут удалены.
    /// </param>
    /// <returns>Провайдер контекста фильтра по категориям.</returns>
    [Obsolete("Используйте перегрузку Create(DataProvider, ILogicalFilterContext).")]
    ILogicalFilterProvider Create(IDataProvider dataProvider, ILogicalFilterContext contextToLoad);

    /// <summary>
    ///     Создает провайдер фильтра элементов по доступным данным.
    /// </summary>
    /// <param name="dataProvider">Провайдер доступных данных для выбора пользователя.</param>
    /// <returns>Провайдер контекста фильтра по категориям.</returns>
    ILogicalFilterProvider Create(DataProvider dataProvider);

    /// <summary>
    ///     Загружает заданный контекст и создает провайдер фильтра элементов по доступным данным.
    /// </summary>
    /// <param name="dataProvider">Провайдер доступных данных для выбора пользователя.</param>
    /// <param name="contextToLoad">
    ///     Контекст для загрузки. Категории и правила фильтрации по параметрам, которых нет в доступных, будут удалены.
    /// </param>
    /// <returns>Провайдер контекста фильтра по категориям.</returns>
    ILogicalFilterProvider Create(DataProvider dataProvider, ILogicalFilterContext contextToLoad);
}
