using Autodesk.Revit.DB;

using dosymep.Revit;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Params;

internal class NamedParam : IParam {
    [JsonConstructor]
    public NamedParam(string name) {
        Name = name;
    }

    [JsonProperty]
    public string Name { get; }

    public ElementId GetId(Document doc) {
        return doc.GetSharedParam(Name)?.Id
               ?? doc.GetProjectParam(Name)?.Id
               ?? doc.GetGlobalParam(Name)?.Id
               ?? ElementId.InvalidElementId;
    }
}
