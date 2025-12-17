using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

/// <summary>
///     Отвечает за вызов правильной перегрузки метода <see cref="IOperator.Create" />.
/// </summary>
internal interface IOperatorToken {
    internal IOperator Operator { get; }
    internal FilterRule Create(ElementId paramId);
}
