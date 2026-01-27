using System.Windows;

using dosymep.SimpleServices;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

/// <summary>
///     Контрол с фильтром по параметрам и с возможностью выбора категорий из доступных в
///     <see cref="LogicalFilterProvider" />
/// </summary>
public partial class PresetCategoriesFilterControl {
    /// <summary>
    ///     Свойство для привязки сервиса контекста фильтра к UI.
    /// </summary>
    public static readonly DependencyProperty LogicalFilterProviderProperty
        = DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(PresetCategoriesFilterControl),
            new PropertyMetadata(null));

    /// <summary>
    ///     Свойство для привязки внешнего сервиса локализации к UI.
    /// </summary>
    public static readonly DependencyProperty LocalizationServiceProperty
        = DependencyProperty.Register(
            nameof(LocalizationService),
            typeof(ILocalizationService),
            typeof(PresetCategoriesFilterControl),
            new PropertyMetadata(null));

    /// <summary>
    ///     Свойство для привязки внешнего сервиса установки языка к UI.
    /// </summary>
    public static readonly DependencyProperty LanguageServiceProperty
        = DependencyProperty.Register(
            nameof(LanguageService),
            typeof(ILanguageService),
            typeof(PresetCategoriesFilterControl),
            new PropertyMetadata(null));

    /// <summary>
    ///     Создает контрол с фильтром по параметрам и с возможностью выбора категорий из доступных в
    ///     <see cref="LogicalFilterProvider" />
    /// </summary>
    public PresetCategoriesFilterControl() {
        InitializeComponent();
    }

    /// <summary>
    ///     Свойство для привязки сервиса контекста фильтра к UI.
    /// </summary>
    public ILogicalFilterProvider LogicalFilterProvider {
        get => (ILogicalFilterProvider) GetValue(LogicalFilterProviderProperty);
        set => SetValue(LogicalFilterProviderProperty, value);
    }

    /// <summary>
    ///     Свойство для установки языка интерфейса.
    /// </summary>
    public ILanguageService? LanguageService {
        get => (ILanguageService) GetValue(LanguageServiceProperty);
        set => SetValue(LanguageServiceProperty, value);
    }

    /// <summary>
    ///     Свойство для переопределения локализации.
    /// </summary>
    public ILocalizationService? LocalizationService {
        get => (ILocalizationService) GetValue(LocalizationServiceProperty);
        set => SetValue(LocalizationServiceProperty, value);
    }
}
