using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal class HasValueVisitor : IVisitor {
    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue) {
        logicalFilter.AddHasValueRule(paramName);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, double paramValue) {
        logicalFilter.AddHasValueRule(paramName);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, int paramValue) {
        logicalFilter.AddHasValueRule(paramName);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, ElementId paramValue) {
        logicalFilter.AddHasValueRule(paramName);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, string paramValue) {
        logicalFilter.AddHasValueRule(paramId);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, double paramValue) {
        logicalFilter.AddHasValueRule(paramId);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, int paramValue) {
        logicalFilter.AddHasValueRule(paramId);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, ElementId paramValue) {
        logicalFilter.AddHasValueRule(paramId);
    }
}
