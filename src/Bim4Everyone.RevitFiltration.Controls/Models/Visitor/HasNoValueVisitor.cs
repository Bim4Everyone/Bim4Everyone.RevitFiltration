using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal class HasNoValueVisitor : IVisitor {
    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue) {
        logicalFilter.AddHasNoValueRule(paramName);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, double paramValue) {
        logicalFilter.AddHasNoValueRule(paramName);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, int paramValue) {
        logicalFilter.AddHasNoValueRule(paramName);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, ElementId paramValue) {
        logicalFilter.AddHasNoValueRule(paramName);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, string paramValue) {
        logicalFilter.AddHasNoValueRule(paramId);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, double paramValue) {
        logicalFilter.AddHasNoValueRule(paramId);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, int paramValue) {
        logicalFilter.AddHasNoValueRule(paramId);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, ElementId paramValue) {
        logicalFilter.AddHasNoValueRule(paramId);
    }
}
