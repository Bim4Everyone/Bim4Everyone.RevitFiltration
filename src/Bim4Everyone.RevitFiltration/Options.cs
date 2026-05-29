namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Настройки сборки фильтра <see cref="Autodesk.Revit.DB.ElementFilter" /> из <see cref="ILogicalFilter" />.
/// </summary>
public sealed class Options {
    /// <summary>
    ///     Допустимая погрешность для значений параметров с плавающей точкой.
    /// </summary>
    public double Tolerance { get; set; } = 1e-6;

    /// <summary>
    ///     Если True, фильтр генерируется по типоразмерам (типам элементов).
    ///     Если False (по умолчанию), фильтр генерируется по экземплярам элементов.
    /// </summary>
    public bool FilterByType { get; set; }
}
