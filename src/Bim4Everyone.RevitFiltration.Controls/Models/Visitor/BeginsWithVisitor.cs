using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

internal class BeginsWithVisitor : IVisitor {
    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, double paramValue) {
        throw new NotImplementedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, int paramValue) {
        throw new NotImplementedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, string paramName, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, double paramValue) {
        throw new NotImplementedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, int paramValue) {
        throw new NotImplementedException();
    }

    public void AddInnerRule(ILogicalFilter logicalFilter, BuiltInParameter paramId, ElementId paramValue) {
        throw new NotImplementedException();
    }
}
