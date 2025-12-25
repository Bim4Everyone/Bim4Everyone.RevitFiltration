using System.Windows;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

/// <summary>
///     Контрол с фильтром по параметрам и с возможностью выбора категорий из доступных в
///     <see cref="ILogicalFilterProvider" />
/// </summary>
public partial class PresetCategoriesFilterControl {
    public static readonly DependencyProperty LogicalFilterProviderProperty
        = DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(PresetCategoriesFilterControl),
            new PropertyMetadata(null));

    public PresetCategoriesFilterControl() {
        InitializeComponent();
    }

    public ILogicalFilterProvider LogicalFilterProvider {
        get => (ILogicalFilterProvider) GetValue(LogicalFilterProviderProperty);
        set => SetValue(LogicalFilterProviderProperty, value);
    }
}
