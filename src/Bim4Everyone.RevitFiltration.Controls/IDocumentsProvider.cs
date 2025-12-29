using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Предоставляет метод получения документов с элементами, по которым будет производиться фильтрация.
/// </summary>
public interface IDocumentsProvider {
    /// <summary>
    ///     Возвращает документы, из которых будут браться существующие значения параметров для выпадающего списка в UI.
    /// </summary>
    /// <returns>Документы со значениями параметров.</returns>
    ICollection<Document> GetDocuments();
}
