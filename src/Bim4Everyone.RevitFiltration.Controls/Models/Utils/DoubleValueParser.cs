using System.Globalization;

using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Utils;

internal class DoubleValueParser {
#if REVIT2021_OR_GREATER
    /// <summary>
    ///     Парсит строковое значение из метрической системы в число в единицах ревита. Если <see cref="unitType" />
    ///     не задан, строка парсится в число без конвертации единиц измерения.
    /// </summary>
    /// <param name="value">Строковое значение в метрической системе.</param>
    /// <param name="unitType">Единицы измерения параметра.</param>
    /// <param name="result">Значение в единицах ревита.</param>
    /// <returns>True, если конвертация строки была успешной, иначе - False.</returns>
    public static bool TryParse(string value, ForgeTypeId unitType, out double result) {
        if(string.IsNullOrEmpty(unitType.TypeId)) {
            return double.TryParse(value, out result);
        }

        return UnitFormatUtils.TryParse(new Units(UnitSystem.Metric), unitType, value, out result);
    }

    /// <summary>
    ///     Форматирует число в единицах ревита в строковое значение в метрической системе.
    ///     Операция, обратная <see cref="TryParse" />. Если <see cref="unitType" />
    ///     не задан, число форматируется без конвертации единиц измерения.
    /// </summary>
    /// <param name="value">Значение в единицах ревита.</param>
    /// <param name="unitType">Единицы измерения параметра.</param>
    /// <returns>Строковое значение в метрической системе.</returns>
    public static string Format(double value, ForgeTypeId unitType) {
        if(string.IsNullOrEmpty(unitType.TypeId)) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        return UnitFormatUtils.Format(new Units(UnitSystem.Metric), unitType, value, true);
    }
#else
    public static bool TryParse(string value, UnitType unitType, out double result) {

        return unitType == UnitType.UT_Undefined
            ? double.TryParse(value, out result)
            : UnitFormatUtils.TryParse(new Units(UnitSystem.Metric), unitType, value, out result);
    }

    /// <summary>
    ///     Форматирует число в единицах ревита в строковое значение в метрической системе.
    ///     Операция, обратная <see cref="TryParse" />. Если <see cref="unitType" />
    ///     не задан, число форматируется без конвертации единиц измерения.
    /// </summary>
    /// <param name="value">Значение в единицах ревита.</param>
    /// <param name="unitType">Единицы измерения параметра.</param>
    /// <returns>Строковое значение в метрической системе.</returns>
    public static string Format(double value, UnitType unitType) {
        return unitType == UnitType.UT_Undefined
            ? value.ToString(CultureInfo.InvariantCulture)
            : UnitFormatUtils.Format(new Units(UnitSystem.Metric), unitType, value, true, true);
    }
#endif
}
