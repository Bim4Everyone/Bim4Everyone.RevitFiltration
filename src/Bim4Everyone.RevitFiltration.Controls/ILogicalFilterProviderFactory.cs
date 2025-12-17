using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Фабрика для создания провайдеров фильтров по категориям.
/// </summary>
public interface ILogicalFilterProviderFactory {
    /// <summary>
    ///     Создает провайдер фильтра элементов по заданным категориям с использованием доступных параметров.
    /// </summary>
    /// <param name="availableCategories">Доступные для фильтрации категории, которые может выбрать пользователь в UI.</param>
    /// <param name="paramsProvider">Провайдер доступных для фильтрации параметров для выбранных пользователем категорий.</param>
    /// <returns>Провайдер контекста фильтра по категориям.</returns>
    ILogicalFilterProvider Create(
        ICollection<Category> availableCategories,
        IParamsProvider paramsProvider);

    /// <summary>
    ///     Загружает заданный контекст и создает провайдер фильтра элементов по заданным категориям с использованием доступных
    ///     параметров.
    /// </summary>
    /// <param name="availableCategories">Доступные для фильтрации категории, которые может выбрать пользователь в UI.</param>
    /// <param name="paramsProvider">Провайдер доступных для фильтрации параметров для выбранных пользователем категорий.</param>
    /// <param name="contextToLoad">
    ///     Контекст для загрузки. Категории и правила фильтрации по параметрам, которых нет в доступных, будут удалены.
    /// </param>
    /// <returns>Провайдер контекста фильтра по категориям.</returns>
    ILogicalFilterProvider Create(
        ICollection<Category> availableCategories,
        IParamsProvider paramsProvider,
        ILogicalFilterContext contextToLoad);
}
