using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

/// <summary>
///     Создает правило "меньше или равно" для фильтрации элементов в Revit.
/// </summary>
internal class LessOrEqualOperator : IOperator {
    /// <inheritdoc />
    public FilterRule Create(ElementId paramId) {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value);
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value, epsilon);
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value);
#endif
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateLessOrEqualRule(paramId, value);
    }
}
