using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

internal class LessOrEqualOperator : IOperator {
    public FilterRule Create(ElementId paramId) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value);
    }

    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value, epsilon);
    }

    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value);
#endif
    }

    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value);
    }
}
