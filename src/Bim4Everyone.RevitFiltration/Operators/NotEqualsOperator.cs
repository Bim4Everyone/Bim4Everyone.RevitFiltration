using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

/// <summary>
///     Создает правило "не равно" для фильтрации элементов в Revit.
/// </summary>
internal class NotEqualsOperator : IOperator {
    /// <inheritdoc />
    public FilterRule Create(ElementId paramId) {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value);
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value, epsilon);
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, string value) {
#if REVIT2023_OR_GREATER
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value);
#else
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value, false);
#endif
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value);
    }
}
