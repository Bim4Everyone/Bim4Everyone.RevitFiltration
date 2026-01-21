using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Интерфейс для получения данных о параметре фильтрации.
/// </summary>
public interface IParam {
    /// <summary>
    ///     Название параметра в Revit.
    /// </summary>
    string Name { get; }

#if REVIT_2020_OR_LESS
    /// <summary>
    ///     Единицы измерения параметра в Revit.
    /// </summary>
    public UnitType UnitType { get; }
#else
    /// <summary>
    ///     Единицы измерения параметра в Revit.
    /// </summary>
    ForgeTypeId UnitType { get; }
#endif

    /// <summary>
    ///     Тип данных параметра.
    /// </summary>
    StorageType StorageType { get; }

    /// <summary>
    ///     Идентификатор параметра в документе Revit.
    /// </summary>
    ElementId Id { get; }
}
