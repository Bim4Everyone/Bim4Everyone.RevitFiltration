using System.Windows;
using System.Windows.Controls;

using Bim4Everyone.RevitFiltration.Controls.ViewModels;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

internal partial class InnerDynamicCategoriesFilterControl : UserControl {
    public static readonly DependencyProperty LogicalFilterProviderProperty =
        DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(InnerDynamicCategoriesFilterControl),
            new PropertyMetadata(null, OnLogicalFilterServiceChanged));

    public InnerDynamicCategoriesFilterControl() {
        InitializeComponent();
    }

    public ILogicalFilterProvider LogicalFilterProvider {
        get => (ILogicalFilterProvider) GetValue(LogicalFilterProviderProperty);
        set => SetValue(LogicalFilterProviderProperty, value);
    }

    private static void OnLogicalFilterServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uc = (InnerDynamicCategoriesFilterControl) d;
        if(uc.DataContext is DynamicCategoriesFilterViewModel vm) {
            var provider = (ILogicalFilterProvider) e.NewValue;
            vm.LoadProvider(provider);
        }
    }
}
