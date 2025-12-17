using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

internal class NotEqualsOperator : IOperator {
    public FilterRule Create(ElementId paramId) {
        throw new NotImplementedException();
    }

    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value);
    }

    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value, epsilon);
    }

    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value);
#endif
    }

    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value);
    }
}
