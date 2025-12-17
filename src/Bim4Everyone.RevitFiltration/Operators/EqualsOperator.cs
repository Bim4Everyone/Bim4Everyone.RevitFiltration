using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

internal class EqualsOperator : IOperator {
    public FilterRule Create(ElementId paramId) {
        throw new NotImplementedException();
    }

    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value);
    }

    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value, epsilon);
    }

    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value);
#endif
    }

    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value);
    }
}
