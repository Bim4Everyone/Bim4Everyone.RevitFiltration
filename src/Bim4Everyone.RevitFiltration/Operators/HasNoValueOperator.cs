using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

internal class HasNoValueOperator : IOperator {
    public FilterRule Create(ElementId paramId) {
        return ParameterFilterRuleFactory.CreateHasNoValueParameterRule(paramId);
    }

    public FilterRule Create(ElementId paramId, int value) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, string value) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, ElementId value) {
        throw new NotSupportedException();
    }
}
