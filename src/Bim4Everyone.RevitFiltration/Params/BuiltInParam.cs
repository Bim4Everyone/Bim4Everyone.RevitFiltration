using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Params;

internal class BuiltInParam : IParam {
    public BuiltInParam(BuiltInParameter param) {
        Param = param;
    }

    public BuiltInParameter Param { get; }

    public ElementId GetId(Document doc) {
        return new ElementId(Param);
    }
}
