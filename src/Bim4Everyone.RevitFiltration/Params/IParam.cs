using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Params;

/// <summary>
///     Предоставляет метод для получения id параметра из документа.
/// </summary>
internal interface IParam {
    /// <summary>
    ///     Получает ElementId параметра из заданного документа Revit.
    /// </summary>
    /// <param name="doc">Документ Revit для поиска параметра.</param>
    /// <returns>Возвращает ElementId найденного параметра.</returns>
    internal ElementId GetId(Document doc);
}
