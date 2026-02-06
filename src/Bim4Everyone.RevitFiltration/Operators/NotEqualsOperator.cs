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
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value);
#endif
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateNotEqualsRule(paramId, value);
    }
}
