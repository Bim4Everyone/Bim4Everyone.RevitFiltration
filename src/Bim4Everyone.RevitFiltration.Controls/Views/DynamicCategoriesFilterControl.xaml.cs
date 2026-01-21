using System.Windows;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

/// <summary>
///     Контрол только с фильтром по параметрам для всех доступных категорий из <see cref="LogicalFilterProvider" />
/// </summary>
public partial class DynamicCategoriesFilterControl {
    /// <summary>
    ///     Свойство для привязки сервиса контекста фильтра к UI.
    /// </summary>
    public static readonly DependencyProperty LogicalFilterProviderProperty
        = DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(DynamicCategoriesFilterControl),
            new PropertyMetadata(null));

    /// <summary>
    ///     Создает контрол только с фильтром по параметрам для всех доступных категорий из
    ///     <see cref="LogicalFilterProvider" />
    /// </summary>
    public DynamicCategoriesFilterControl() {
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
