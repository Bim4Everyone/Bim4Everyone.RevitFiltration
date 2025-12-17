using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

internal class GreaterOrEqualOperator : IOperator {
    public FilterRule Create(ElementId paramId) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateGreaterOrEqualRule(paramId, value);
    }

    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateGreaterOrEqualRule(paramId, value, epsilon);
    }

    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateGreaterOrEqualRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateGreaterOrEqualRule(paramId, value);
#endif
    }

    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateGreaterOrEqualRule(paramId, value);
    }
}
