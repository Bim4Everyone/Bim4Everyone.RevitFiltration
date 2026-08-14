using Autodesk.Revit.DB;

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
    ///     Контекст для загрузки. Если хотя бы одна из категорий недоступна либо хотя бы одно правило
    ///     не удается сопоставить с доступными параметрами, контекст не загружается: будет создан провайдер с пустым контекстом
    ///     с <see cref="ILogicalFilterProvider.CanGetFilter" /> = False.
    /// </param>
    /// <returns>Провайдер контекста фильтра по категориям.</returns>
    ILogicalFilterProvider Create(DataProvider dataProvider, ILogicalFilterContext contextToLoad);

    /// <summary>
    ///     Загружает заданный фильтр с заданными категориями
    ///     и создает провайдер фильтра элементов по доступным данным.
    /// </summary>
    /// <param name="dataProvider">Провайдер доступных данных для выбора пользователя.</param>
    /// <param name="filterToLoad">
    ///     Фильтр для загрузки. Если хотя бы одна из заданных категорий недоступна либо хотя бы одно правило
    ///     не удается сопоставить с доступными параметрами, фильтр не загружается: будет создан провайдер с пустым контекстом
    ///     с <see cref="ILogicalFilterProvider.CanGetFilter" /> = False.
    /// </param>
    /// <param name="categories">Категории элементов, для которых задан фильтр.</param>
    /// <returns>Провайдер контекста фильтра по категориям.</returns>
    ILogicalFilterProvider Create(
        DataProvider dataProvider,
        ILogicalFilter filterToLoad,
        ICollection<BuiltInCategory> categories);
}
