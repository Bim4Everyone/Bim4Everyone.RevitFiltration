using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Создает правило фильтрации элементов в Revit (больше, больше или равно, меньше, содержит, без значения и т.д.).
/// </summary>
internal interface IOperator {
    internal OperatorKind OperatorKind { get; }
    internal FilterRule Create(ElementId paramId);
    internal FilterRule Create(ElementId paramId, int value);
    internal FilterRule Create(ElementId paramId, double value);
    internal FilterRule Create(ElementId paramId, string value);
    internal FilterRule Create(ElementId paramId, ElementId value);
}
