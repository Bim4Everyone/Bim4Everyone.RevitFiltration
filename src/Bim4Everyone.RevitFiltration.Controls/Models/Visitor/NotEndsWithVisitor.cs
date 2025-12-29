using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal class NotEndsWithVisitor : IVisitor {
    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue) {
        logicalFilter.AddNotEndsWithRule(paramName, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, double paramValue) {
        throw new NotSupportedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, int paramValue) {
        throw new NotSupportedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, ElementId paramValue) {
        throw new NotSupportedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, string paramValue) {
        logicalFilter.AddNotEndsWithRule(paramId, paramValue);
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, double paramValue) {
        throw new NotSupportedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, int paramValue) {
        throw new NotSupportedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, ElementId paramValue) {
        throw new NotSupportedException();
    }
}
