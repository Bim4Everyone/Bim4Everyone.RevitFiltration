using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Compositors;

/// <summary>
///     Создает фильтр с логическим правилом "И" или "ИЛИ".
/// </summary>
internal interface ICompositor {
    internal ElementFilter Create(ICollection<ElementFilter> filters);

    /// <summary>
    ///     Возвращает противоположный композитор: "ИЛИ" для "И" и наоборот.
    ///     Нужен для построения инвертированного фильтра по законам де Моргана.
    /// </summary>
    /// <returns>Противоположный композитор.</returns>
    internal ICompositor Invert();
}
