using Autodesk.Revit.DB;

using dosymep.Bim4Everyone;

namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Провайдер данных.
/// </summary>
public interface IDataProvider {
    /// <summary>
    ///     Возвращает параметры, доступные сразу для всех заданных категорий.
    /// </summary>
    /// <param name="categories">Заданные категории.</param>
    /// <returns>Коллекция доступных параметров.</returns>
    ICollection<RevitParam> GetParams(ICollection<Category> categories);

    /// <summary>
    ///     Возвращает категории, доступные для фильтрации.
    /// </summary>
    /// <returns>Коллекция доступных для выбора категорий.</returns>
    ICollection<Category> GetCategories();

    /// <summary>
    ///     Возвращает документы, из которых будут браться существующие значения параметров для выпадающего списка в UI.
    /// </summary>
    /// <returns>Документы со значениями параметров.</returns>
    ICollection<Document> GetDocuments();
}
