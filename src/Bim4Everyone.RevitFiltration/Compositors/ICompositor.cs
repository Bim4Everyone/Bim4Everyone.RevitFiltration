using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Compositors;

/// <summary>
///     Создает правило для "И" и "Или".
/// </summary>
internal interface ICompositor {
    internal ElementFilter Create(ICollection<ElementFilter> filters);
}
