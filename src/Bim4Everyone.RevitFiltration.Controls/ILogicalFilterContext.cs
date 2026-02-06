using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Контекст фильтра элементов по категориям и правилам.
/// </summary>
public interface ILogicalFilterContext {
    /// <summary>
    ///     Выбранные пользователем категории элементов для фильтрации.
    /// </summary>
    ICollection<BuiltInCategory> SelectedCategories { get; }

    internal Filter Filter { get; }

    /// <summary>
    ///     Правила фильтрации значений параметров.
    /// </summary>
    ILogicalFilter GetFilter();
}
