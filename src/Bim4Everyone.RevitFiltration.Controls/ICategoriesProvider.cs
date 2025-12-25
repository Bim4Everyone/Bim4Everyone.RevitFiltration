using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls;

public interface ICategoriesProvider {
    /// <summary>
    ///     Возвращает категории, доступные для фильтрации.
    /// </summary>
    /// <returns>Коллекция доступных для выбора категорий.</returns>
    ICollection<Category> GetCategories();
}
