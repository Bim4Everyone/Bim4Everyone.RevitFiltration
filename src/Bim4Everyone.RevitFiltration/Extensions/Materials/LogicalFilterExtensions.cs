using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Filtration;

namespace Bim4Everyone.RevitFiltration.Extensions.Materials;

/// <summary>
///     Расширения <see cref="ILogicalFilter" /> для фильтрации элементов по параметрам их материалов.
/// </summary>
public static class LogicalFilterExtensions {
    /// <summary>
    ///     Отбирает материалы документа, которые проходят правила фильтра,
    ///     и создает фильтр элементов по этим материалам.
    ///     Значения параметров материалов сравниваются напрямую, без создания фильтров Revit.
    /// </summary>
    /// <param name="filter">Правила отбора материалов по значениям их параметров.</param>
    /// <param name="doc">Документ, в котором отбираются материалы.</param>
    /// <param name="opts">Настройки отбора материалов.</param>
    /// <returns>Фильтр элементов по материалам.</returns>
    /// <remarks>
    ///     Материалы отбираются сразу, за один проход по документу,
    ///     поэтому созданный <see cref="MaterialsFilter" /> в документ больше не обращается
    ///     и не увидит изменений, сделанных после его создания.
    ///     Фильтру без единого правила удовлетворяет любой элемент.
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">Любой из аргументов null.</exception>
    /// <exception cref="System.InvalidOperationException">Исключение, если нельзя построить фильтр по материалам.
    /// </exception>
    public static MaterialsFilter BuildMaterialsFilter(
        this ILogicalFilter filter,
        Document doc,
        MaterialOptions opts) {
        if(filter == null) {
            throw new ArgumentNullException(nameof(filter));
        }

        if(doc == null) {
            throw new ArgumentNullException(nameof(doc));
        }

        if(opts == null) {
            throw new ArgumentNullException(nameof(opts));
        }

        if(filter is not LogicalFilter logicalFilter) {
            throw new InvalidOperationException("Нельзя построить фильтр по материалам");
        }

        if(logicalFilter.InnerRules.Count == 0
           && logicalFilter.InnerFilters.Count == 0) {
            // правил нет - фильтр проходят все элементы
            return new MaterialsFilter();
        }

        var evaluator = new MaterialsFilterEvaluator(doc, logicalFilter, opts);
        var materialIds = new FilteredElementCollector(doc)
            .WhereElementIsNotElementType()
            .OfClass(typeof(Material))
            .ToElements()
            .Where(evaluator.Evaluate)
            .Select(m => m.Id)
            .ToArray();
        return new MaterialsFilter(materialIds);
    }
}
