using System.Windows;
using System.Windows.Controls;

using Bim4Everyone.RevitFiltration.Controls.ViewModels;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

internal partial class InnerPresetCategoriesFilterControl : UserControl {
    public static readonly DependencyProperty LogicalFilterProviderProperty =
        DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(InnerPresetCategoriesFilterControl),
            new PropertyMetadata(null, OnLogicalFilterServiceChanged));

    public InnerPresetCategoriesFilterControl() {
        InitializeComponent();
    }

    public ILogicalFilterProvider LogicalFilterProvider {
        get => (ILogicalFilterProvider) GetValue(LogicalFilterProviderProperty);
        set => SetValue(LogicalFilterProviderProperty, value);
    }

    private static void OnLogicalFilterServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uc = (InnerPresetCategoriesFilterControl) d;
        if(uc.DataContext is DynamicCategoriesFilterViewModel vm) {
            var provider = (ILogicalFilterProvider) e.NewValue;
            vm.LoadProvider(provider);
            vm.AllCategoriesSelected = true;
        }
    }
}
