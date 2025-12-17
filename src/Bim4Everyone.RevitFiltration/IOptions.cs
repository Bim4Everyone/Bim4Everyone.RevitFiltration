namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Настройки сборки фильтра <see cref="Autodesk.Revit.DB.ElementFilter" /> из <see cref="ILogicalFilter" />.
/// </summary>
public interface IOptions {
    /// <summary>
    ///     Допустимая погрешность для значений параметров с плавающей точкой.
    /// </summary>
    double Tolerance { get; set; }
}
