using System.Windows;

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
}
