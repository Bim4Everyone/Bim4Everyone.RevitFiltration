using System.Windows;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

/// <summary>
///     Контрол только с фильтром по параметрам для всех доступных категорий из <see cref="ILogicalFilterProvider" />
/// </summary>
public partial class DynamicCategoriesFilterControl {
    public static readonly DependencyProperty LogicalFilterProviderProperty
        = DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(DynamicCategoriesFilterControl),
            new PropertyMetadata(null));

    public DynamicCategoriesFilterControl() {
        InitializeComponent();
    }

    public ILogicalFilterProvider LogicalFilterProvider {
        get => (ILogicalFilterProvider) GetValue(LogicalFilterProviderProperty);
        set => SetValue(LogicalFilterProviderProperty, value);
    }
}
