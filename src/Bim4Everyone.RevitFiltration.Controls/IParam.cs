using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls;

public interface IParam {
    string Name { get; }

#if REVIT_2020_OR_LESS
    public UnitType UnitType { get; }
#else
    ForgeTypeId UnitType { get; }
#endif

    StorageType StorageType { get; }

    ElementId Id { get; }
}
