using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal class LessOrEqualVisitor : IVisitor {
    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue) {
        logicalFilter.AddLessOrEqualRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, double paramValue) {
        logicalFilter.AddLessOrEqualRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, int paramValue) {
        logicalFilter.AddLessOrEqualRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, ElementId paramValue) {
        logicalFilter.AddLessOrEqualRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, string paramValue) {
        logicalFilter.AddLessOrEqualRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, double paramValue) {
        logicalFilter.AddLessOrEqualRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, int paramValue) {
        logicalFilter.AddLessOrEqualRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, ElementId paramValue) {
        logicalFilter.AddLessOrEqualRule(paramId, paramValue);
    }
}
