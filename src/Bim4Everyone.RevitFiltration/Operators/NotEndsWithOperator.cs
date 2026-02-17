using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

/// <summary>
///     Создает правило "не заканчивается на" для фильтрации элементов в Revit.
/// </summary>
internal class NotEndsWithOperator : IOperator {
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
        return ParameterFilterRuleFactory.CreateNotEndsWithRule(paramId, value);
#else
        return ParameterFilterRuleFactory.CreateNotEndsWithRule(paramId, value, false);
#endif
    }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, ElementId value) {
        throw new NotSupportedException();
    }
}
