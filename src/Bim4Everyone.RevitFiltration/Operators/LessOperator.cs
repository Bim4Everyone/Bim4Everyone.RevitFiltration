using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

internal class LessOperator : IOperator {
    public FilterRule Create(ElementId paramId) {
        throw new NotSupportedException();
    }

    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateLessRule(paramId, value);
    }

    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateLessRule(paramId, value, epsilon);
    }

    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateLessRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateLessRule(paramId, value);
#endif
    }

    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateLessRule(paramId, value);
    }
}
