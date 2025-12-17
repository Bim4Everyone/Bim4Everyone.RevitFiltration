using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Создает правила фильтрации элементов в Autodesk Revit.
/// </summary>
public interface ILogicalFilter {
    internal ICompositor Compositor { get; }

    /// <summary>
    ///     Создает фильтр по элементам в Autodesk Revit с заданными настройками.
    /// </summary>
    /// <param name="document">Документ, в котором будут фильтроваться элементы.</param>
    /// <param name="options">Настройки генерации фильтра.</param>
    /// <returns>Фильтр по элементам.</returns>
    ElementFilter Build(Document document, IOptions options);

    /// <summary>
    ///     Добавляет вложенный ILogicalFilter.
    /// </summary>
    /// <param name="innerFilter">Вложенный фильтр.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddFilter(ILogicalFilter innerFilter);

    ILogicalFilter AddGreaterOrEqualRule(string paramName, int paramValue);
    ILogicalFilter AddGreaterOrEqualRule(string paramName, double paramValue);
    ILogicalFilter AddGreaterOrEqualRule(string paramName, string paramValue);
    ILogicalFilter AddGreaterOrEqualRule(string paramName, ElementId paramValue);
    ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, int paramValue);
    ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, double paramValue);
    ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, string paramValue);
    ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, ElementId paramValue);

    ILogicalFilter AddGreaterRule(string paramName, int paramValue);
    ILogicalFilter AddGreaterRule(string paramName, double paramValue);
    ILogicalFilter AddGreaterRule(string paramName, string paramValue);
    ILogicalFilter AddGreaterRule(string paramName, ElementId paramValue);
    ILogicalFilter AddGreaterRule(BuiltInParameter paramId, int paramValue);
    ILogicalFilter AddGreaterRule(BuiltInParameter paramId, double paramValue);
    ILogicalFilter AddGreaterRule(BuiltInParameter paramId, string paramValue);
    ILogicalFilter AddGreaterRule(BuiltInParameter paramId, ElementId paramValue);

    ILogicalFilter AddLessOrEqualRule(string paramName, int paramValue);
    ILogicalFilter AddLessOrEqualRule(string paramName, double paramValue);
    ILogicalFilter AddLessOrEqualRule(string paramName, string paramValue);
    ILogicalFilter AddLessOrEqualRule(string paramName, ElementId paramValue);
    ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, int paramValue);
    ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, double paramValue);
    ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, string paramValue);
    ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, ElementId paramValue);

    ILogicalFilter AddLessRule(string paramName, int paramValue);
    ILogicalFilter AddLessRule(string paramName, double paramValue);
    ILogicalFilter AddLessRule(string paramName, string paramValue);
    ILogicalFilter AddLessRule(string paramName, ElementId paramValue);
    ILogicalFilter AddLessRule(BuiltInParameter paramId, int paramValue);
    ILogicalFilter AddLessRule(BuiltInParameter paramId, double paramValue);
    ILogicalFilter AddLessRule(BuiltInParameter paramId, string paramValue);
    ILogicalFilter AddLessRule(BuiltInParameter paramId, ElementId paramValue);

    ILogicalFilter AddEqualsRule(string paramName, int paramValue);
    ILogicalFilter AddEqualsRule(string paramName, double paramValue);
    ILogicalFilter AddEqualsRule(string paramName, string paramValue);
    ILogicalFilter AddEqualsRule(string paramName, ElementId paramValue);
    ILogicalFilter AddEqualsRule(BuiltInParameter paramId, int paramValue);
    ILogicalFilter AddEqualsRule(BuiltInParameter paramId, double paramValue);
    ILogicalFilter AddEqualsRule(BuiltInParameter paramId, string paramValue);
    ILogicalFilter AddEqualsRule(BuiltInParameter paramId, ElementId paramValue);

    ILogicalFilter AddNotEqualsRule(string paramName, int paramValue);
    ILogicalFilter AddNotEqualsRule(string paramName, double paramValue);
    ILogicalFilter AddNotEqualsRule(string paramName, string paramValue);
    ILogicalFilter AddNotEqualsRule(string paramName, ElementId paramValue);
    ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, int paramValue);
    ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, double paramValue);
    ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, string paramValue);
    ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, ElementId paramValue);

    ILogicalFilter AddHasNoValueRule(string paramName);
    ILogicalFilter AddHasNoValueRule(BuiltInParameter paramId);

    ILogicalFilter AddHasValueRule(string paramName);
    ILogicalFilter AddHasValueRule(BuiltInParameter paramId);

    ILogicalFilter AddBeginsWithRule(string paramName, string paramValue);
    ILogicalFilter AddBeginsWithRule(BuiltInParameter paramId, string paramValue);

    ILogicalFilter AddNotBeginsWithRule(string paramName, string paramValue);
    ILogicalFilter AddNotBeginsWithRule(BuiltInParameter paramId, string paramValue);

    ILogicalFilter AddEndsWithRule(string paramName, string paramValue);
    ILogicalFilter AddEndsWithRule(BuiltInParameter paramId, string paramValue);

    ILogicalFilter AddNotEndsWithRule(string paramName, string paramValue);
    ILogicalFilter AddNotEndsWithRule(BuiltInParameter paramId, string paramValue);

    ILogicalFilter AddContainsRule(string paramName, string paramValue);
    ILogicalFilter AddContainsRule(BuiltInParameter paramId, string paramValue);

    ILogicalFilter AddNotContainsRule(string paramName, string paramValue);
    ILogicalFilter AddNotContainsRule(BuiltInParameter paramId, string paramValue);
}
