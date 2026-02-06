using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal interface IVisitor {
    void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue);
    void AddInnerRule(ILogicalFilter logicalFilter, string paramName, double paramValue);
    void AddInnerRule(ILogicalFilter logicalFilter, string paramName, int paramValue);
    void AddInnerRule(ILogicalFilter logicalFilter, string paramName, ElementId paramValue);
    void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, string paramValue);
    void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, double paramValue);
    void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, int paramValue);
    void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, ElementId paramValue);
}
