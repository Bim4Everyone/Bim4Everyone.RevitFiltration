using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal class EqualsVisitor : IVisitor {
    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue) {
        logicalFilter.AddEqualsRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, double paramValue) {
        logicalFilter.AddEqualsRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, int paramValue) {
        logicalFilter.AddEqualsRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, ElementId paramValue) {
        logicalFilter.AddEqualsRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, string paramValue) {
        logicalFilter.AddEqualsRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, double paramValue) {
        logicalFilter.AddEqualsRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, int paramValue) {
        logicalFilter.AddEqualsRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, ElementId paramValue) {
        logicalFilter.AddEqualsRule(paramId, paramValue);
    }
}
