using Autodesk.Revit.DB;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Params;

internal class BuiltInParam : IParam {
    [JsonConstructor]
    public BuiltInParam(BuiltInParameter param) {
        Param = param;
    }

    [JsonProperty]
    public BuiltInParameter Param { get; }

    public ElementId GetId(Document doc) {
        return new ElementId(Param);
    }
}
