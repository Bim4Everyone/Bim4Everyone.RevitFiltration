using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Предоставляет метод для получения id параметра из документа.
/// </summary>
internal interface IParam {
    internal ElementId GetId(Document doc);
}
