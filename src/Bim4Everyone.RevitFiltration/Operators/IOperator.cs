using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

/// <summary>
///     Создает правило фильтрации элементов в Revit (больше, больше или равно, меньше, содержит, без значения и т.д.).
/// </summary>
internal interface IOperator {
    internal FilterRule Create(ElementId paramId);
    internal FilterRule Create(ElementId paramId, int value);
    internal FilterRule Create(ElementId paramId, double value, double epsilon);
    internal FilterRule Create(ElementId paramId, string value);
    internal FilterRule Create(ElementId paramId, ElementId value);
}
