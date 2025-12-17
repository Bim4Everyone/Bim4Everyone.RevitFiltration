using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

internal class GreaterOperator : IOperator {
    public FilterRule Create(ElementId paramId) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateGreaterRule(paramId, value);
    }

    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateGreaterRule(paramId, value, epsilon);
    }

    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateGreaterRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateGreaterRule(paramId, value);
#endif
    }

    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateGreaterRule(paramId, value);
    }
}
