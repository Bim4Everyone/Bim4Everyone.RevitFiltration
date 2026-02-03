using System.Windows;
using System.Windows.Controls;

using Bim4Everyone.RevitFiltration.Controls.ViewModels;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

internal partial class CategoriesView : UserControl {
    public CategoriesView() {
        InitializeComponent();
        this.DataContextChanged += OnDataContextChanged;
    }
    
    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
        if(e.NewValue is DynamicCategoriesFilterViewModel vm) {
            this.Resources["NameHeader"] = vm.NameHeader;
        }
    }
}
