using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

/// <summary>
///     Создает правило "не сожержит" для фильтрации элементов в Revit.
/// </summary>
internal class NotContainsOperator : IOperator {
    /// <inheritdoc />
    public FilterRule Create(ElementId paramId) {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, int value) {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, double value, double epsilon) {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, string value) {
#if REVIT2023_OR_GREATER
        return ParameterFilterRuleFactory.CreateNotContainsRule(paramId, value);
#else
        return ParameterFilterRuleFactory.CreateNotContainsRule(paramId, value, false);
#endif
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, ElementId value) {
        throw new NotSupportedException();
    }
}
