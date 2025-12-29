using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal class GreaterVisitor : IVisitor {
    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue) {
        logicalFilter.AddGreaterRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, double paramValue) {
        logicalFilter.AddGreaterRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, int paramValue) {
        logicalFilter.AddGreaterRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, ElementId paramValue) {
        logicalFilter.AddGreaterRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, string paramValue) {
        logicalFilter.AddGreaterRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, double paramValue) {
        logicalFilter.AddGreaterRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, int paramValue) {
        logicalFilter.AddGreaterRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, ElementId paramValue) {
        logicalFilter.AddGreaterRule(paramId, paramValue);
    }
}
