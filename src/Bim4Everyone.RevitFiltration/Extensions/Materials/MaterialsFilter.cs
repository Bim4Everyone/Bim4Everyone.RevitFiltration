using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Extensions.Materials;

/// <summary>
///     Фильтр элементов по материалам, которые в них используются.
///     Создается через <see cref="LogicalFilterExtensions.BuildMaterialsFilter" />.
/// </summary>
public sealed class MaterialsFilter {
    private readonly HashSet<ElementId> _materialIds;
    private readonly bool _passesAll;

    internal MaterialsFilter(ICollection<ElementId> materialIds) {
        if(materialIds == null) {
            throw new ArgumentNullException(nameof(materialIds));
        }

        _materialIds = [..materialIds];
    }

    /// <summary>
    ///     Создает фильтр без правил, который пропускает любой элемент.
    /// </summary>
    internal MaterialsFilter() {
        _materialIds = [];
        _passesAll = true;
    }

    /// <summary>
    ///     Проверяет, используется ли в элементе хотя бы один из отобранных материалов.
    ///     Учитываются как материалы самого элемента, так и материалы краски его граней.
    /// </summary>
    /// <param name="element">Проверяемый элемент.</param>
    /// <returns>True, если элемент проходит фильтр, иначе - False.</returns>
    /// <exception cref="System.ArgumentNullException">element is null.</exception>
    public bool PassesFilter(Element element) {
        if(element == null) {
            throw new ArgumentNullException(nameof(element));
        }

        if(_passesAll) {
            return true;
        }

        if(_materialIds.Count == 0) {
            // ни один материал не подошел - значит и ни один элемент не пройдет
            return false;
        }

        return element.GetMaterialIds(false).Any(_materialIds.Contains)
               || element.GetMaterialIds(true).Any(_materialIds.Contains);
    }

    /// <summary>
    ///     Возвращает элементы, которые проходят фильтр.
    /// </summary>
    /// <param name="elements">Проверяемые элементы.</param>
    /// <returns>Элементы, прошедшие фильтр.</returns>
    /// <exception cref="System.ArgumentNullException">elements is null.</exception>
    public IEnumerable<Element> PassesFilter(IEnumerable<Element> elements) {
        if(elements == null) {
            throw new ArgumentNullException(nameof(elements));
        }

        return elements.Where(PassesFilter);
    }
}
