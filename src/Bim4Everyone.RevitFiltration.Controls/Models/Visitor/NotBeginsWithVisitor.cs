using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal class NotBeginsWithVisitor : IVisitor {
    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue) {
        logicalFilter.AddNotBeginsWithRule(paramName, paramValue);
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
        logicalFilter.AddNotBeginsWithRule(paramId, paramValue);
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
