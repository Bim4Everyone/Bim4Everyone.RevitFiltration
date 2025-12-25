using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Отвечает за вызов правильной перегрузки метода <see cref="IOperator.Create" />.
/// </summary>
internal interface IOperatorToken {
    internal IOperator Operator { get; }
    internal string? DisplayExpectedValue { get; }
    internal FilterRule Create(ElementId paramId);
}
