using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

internal class ContainsOperator : IOperator {
    public FilterRule Create(ElementId paramId) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, int value) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateContainsRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateContainsRule(paramId, value);
#endif
    }

    public FilterRule Create(ElementId paramId, ElementId value) {
        throw new NotSupportedException();
    }
}
