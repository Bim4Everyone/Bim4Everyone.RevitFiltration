using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

/// <summary>
///     Создает правило "равно" для фильтрации элементов в Revit.
/// </summary>
internal class EqualsOperator : IOperator {
    /// <inheritdoc />
    public FilterRule Create(ElementId paramId) {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, int value) {
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value);
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value, epsilon);
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, string value) {
#if REVIT_2022_OR_LESS
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value, false);
#else
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value);
#endif
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, ElementId value) {
        return ParameterFilterRuleFactory.CreateEqualsRule(paramId, value);
    }
}
