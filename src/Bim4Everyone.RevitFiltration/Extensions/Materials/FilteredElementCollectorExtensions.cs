using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Extensions.Materials;

/// <summary>
///     Расширения <see cref="FilteredElementCollector" /> для фильтрации элементов по их материалам.
/// </summary>
public static class FilteredElementCollectorExtensions {
    /// <summary>
    ///     Возвращает элементы коллектора, которые проходят фильтр по материалам.
    /// </summary>
    /// <param name="collector">Коллектор элементов.</param>
    /// <param name="filter">Фильтр по материалам.</param>
    /// <returns>Элементы, прошедшие фильтр.</returns>
    /// <exception cref="System.ArgumentNullException">Любой из аргументов null.</exception>
    public static IEnumerable<Element> PassesFilter(
        this FilteredElementCollector collector,
        MaterialsFilter filter) {
        if(collector == null) {
            throw new ArgumentNullException(nameof(collector));
        }

        if(filter == null) {
            throw new ArgumentNullException(nameof(filter));
        }

        return filter.PassesFilter(collector);
    }
}
