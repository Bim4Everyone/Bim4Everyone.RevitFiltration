using System.Windows;
using System.Windows.Controls;

using Bim4Everyone.RevitFiltration.Controls.ViewModels;

using dosymep.SimpleServices;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

internal partial class InnerDynamicCategoriesFilterControl : UserControl {
    public static readonly DependencyProperty LogicalFilterProviderProperty =
        DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(InnerDynamicCategoriesFilterControl),
            new PropertyMetadata(null, OnLogicalFilterProviderChanged));

    public static readonly DependencyProperty LocalizationServiceProperty =
        DependencyProperty.Register(
            nameof(LocalizationService),
            typeof(ILocalizationService),
            typeof(InnerDynamicCategoriesFilterControl),
            new PropertyMetadata(null, OnLocalizationServiceChanged));

    public static readonly DependencyProperty LanguageServiceProperty =
        DependencyProperty.Register(
            nameof(LanguageService),
            typeof(ILanguageService),
            typeof(InnerDynamicCategoriesFilterControl),
            new PropertyMetadata(null, OnLanguageServiceChanged));

    public InnerDynamicCategoriesFilterControl() {
        InitializeComponent();
    }

    public ILogicalFilterProvider LogicalFilterProvider {
        get => (ILogicalFilterProvider) GetValue(LogicalFilterProviderProperty);
        set => SetValue(LogicalFilterProviderProperty, value);
    }

    public ILocalizationService LocalizationService {
        get => (ILocalizationService) GetValue(LocalizationServiceProperty);
        set => SetValue(LocalizationServiceProperty, value);
    }

    public ILanguageService LanguageService {
        get => (ILanguageService) GetValue(LanguageServiceProperty);
        set => SetValue(LanguageServiceProperty, value);
    }

    private static void OnLogicalFilterProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uc = (InnerDynamicCategoriesFilterControl) d;
        if(uc.DataContext is DynamicCategoriesFilterViewModel vm) {
            var provider = (ILogicalFilterProvider) e.NewValue;
            vm.LoadProvider(provider);
        }
    }

    private static void OnLocalizationServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uc = (InnerDynamicCategoriesFilterControl) d;
        if(uc.DataContext is DynamicCategoriesFilterViewModel vm) {
            var localizationService = (ILocalizationService) e.NewValue;
            vm.LocalizationProvider.OutsourceLocalization = localizationService;
        }
    }

    private static void OnLanguageServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uc = (InnerDynamicCategoriesFilterControl) d;
        if(uc.DataContext is DynamicCategoriesFilterViewModel vm) {
            var languageService = (ILanguageService) e.NewValue;
            if(languageService != null) {
                vm.LocalizationProvider.SetInnerLocalization(languageService.HostLanguage);
            }
        }
    }
}
