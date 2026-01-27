using System.Globalization;

using dosymep.SimpleServices;

namespace Bim4Everyone.RevitFiltration.Controls.Services;

internal interface ILocalizationProvider {
    /// <summary>
    ///     Внутренняя локализация либы.
    /// </summary>
    ILocalizationService InnerLocalization { get; }

    /// <summary>
    ///     Внешняя локализация, переопределяющая внутреннюю.
    /// </summary>
    ILocalizationService? OutsourceLocalization { get; set; }

    /// <summary>
    ///     Возвращает строку локализации. Поиск строки локализации будет сначала по <see cref="OutsourceLocalization" />, если
    ///     это свойство указано.
    /// </summary>
    /// <param name="name">Наименование локализированной строки.</param>
    /// <returns>Возвращает локализированную строку.</returns>
    string GetLocalizedString(string name);

    /// <summary>
    ///     Возвращает форматированную строку локализации. Поиск строки локализации будет сначала по
    ///     <see cref="OutsourceLocalization" />, если
    ///     это свойство указано.
    /// </summary>
    /// <param name="name">Наименование локализированной строки.</param>
    /// <param name="args">Параметры форматирования локализированной строки.</param>
    /// <returns>Возвращает форматированную локализированную строку.</returns>
    string GetLocalizedString(string name, params object[] args);

    /// <summary>
    ///     Устанавливает внутренние ресурсы локализации для интерфейса.
    /// </summary>
    /// <param name="cultureInfo">Применяемые языковые стандарты.</param>
    void SetInnerLocalization(CultureInfo cultureInfo);

    /// <summary>
    ///     Событие, которое вызывается при смене языковых стандартов или при смене <see cref="OutsourceLocalization" />
    /// </summary>
    event EventHandler LocalizationChanged;
}
