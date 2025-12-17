using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Контекст фильтра элементов по категориям и правилам.
/// </summary>
public interface ILogicalFilterContext {
    /// <summary>
    ///     Правила фильтрации значений параметров.
    /// </summary>
    ILogicalFilter Filter { get; }

    /// <summary>
    ///     Выбранные пользователем категории элементов для фильтрации.
    /// </summary>
    ICollection<BuiltInCategory> SelectedCategories { get; }
}
