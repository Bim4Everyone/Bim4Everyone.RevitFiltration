using Autodesk.Revit.DB;

using dosymep.Revit;

namespace Bim4Everyone.RevitFiltration.Params;

internal class NamedParam : IParam {
    public NamedParam(string name) {
        Name = name;
    }

    public string Name { get; }

    public ElementId GetId(Document doc) {
        return doc.GetSharedParam(Name)?.Id
               ?? doc.GetProjectParam(Name)?.Id
               ?? doc.GetGlobalParam(Name)?.Id
               ?? ElementId.InvalidElementId;
    }
}
