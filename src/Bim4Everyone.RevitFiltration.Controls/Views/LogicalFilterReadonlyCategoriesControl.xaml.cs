using System.Windows;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

/// <summary>
///     Контрол только с фильтром по параметрам для всех доступных категорий из <see cref="ILogicalFilterProvider" />
/// </summary>
public partial class LogicalFilterReadonlyCategoriesControl {
    public static readonly DependencyProperty LogicalFilterProviderProperty
        = DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(LogicalFilterReadonlyCategoriesControl),
            new PropertyMetadata(null));

    public LogicalFilterReadonlyCategoriesControl() {
        InitializeComponent();
    }

    public ILogicalFilterProvider LogicalFilterProvider {
        get => (ILogicalFilterProvider) GetValue(LogicalFilterProviderProperty);
        set => SetValue(LogicalFilterProviderProperty, value);
    }
}
