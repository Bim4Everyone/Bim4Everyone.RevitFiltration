using Autodesk.Revit.DB;

using dosymep.Revit;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Params;

/// <summary>
///     Класс для работы с пользовательским параметром Revit: общим, проекта или глобальным.
/// </summary>
internal class UserParam : IParam {
    /// <summary>
    ///     Создание на основе имени параметра в проекте Revit.
    /// </summary>
    /// <param name="name">Имя искомого параметра в проекте Revit.</param>
    [JsonConstructor]
    public UserParam(string name) {
        Name = name;
    }

    /// <summary>
    ///     Имя параметра в проекте Revit.
    /// </summary>
    [JsonProperty]
    public string Name { get; }

    /// <inheritdoc />
    public ElementId GetId(Document doc) {
        return doc.GetSharedParam(Name)?.Id
               ?? doc.GetProjectParam(Name)?.Id
               ?? doc.GetGlobalParam(Name)?.Id
               ?? ElementId.InvalidElementId;
    }
}
