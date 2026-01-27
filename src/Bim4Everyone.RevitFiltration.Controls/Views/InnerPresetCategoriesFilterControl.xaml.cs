using System.Windows;
using System.Windows.Controls;

using Bim4Everyone.RevitFiltration.Controls.ViewModels;

using dosymep.SimpleServices;

namespace Bim4Everyone.RevitFiltration.Controls.Views;

internal partial class InnerPresetCategoriesFilterControl : UserControl {
    public static readonly DependencyProperty LogicalFilterProviderProperty =
        DependencyProperty.Register(
            nameof(LogicalFilterProvider),
            typeof(ILogicalFilterProvider),
            typeof(InnerPresetCategoriesFilterControl),
            new PropertyMetadata(null, OnLogicalFilterServiceChanged));

    public static readonly DependencyProperty LocalizationServiceProperty =
        DependencyProperty.Register(
            nameof(LocalizationService),
            typeof(ILocalizationService),
            typeof(InnerPresetCategoriesFilterControl),
            new PropertyMetadata(null, OnLocalizationServiceChanged));

    public static readonly DependencyProperty LanguageServiceProperty =
        DependencyProperty.Register(
            nameof(LanguageService),
            typeof(ILanguageService),
            typeof(InnerPresetCategoriesFilterControl),
            new PropertyMetadata(null, OnLanguageServiceChanged));

    public InnerPresetCategoriesFilterControl() {
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

    private static void OnLogicalFilterServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uc = (InnerPresetCategoriesFilterControl) d;
        if(uc.DataContext is DynamicCategoriesFilterViewModel vm) {
            var provider = (ILogicalFilterProvider) e.NewValue;
            vm.LoadProvider(provider);
            vm.AllCategoriesSelected = true;
        }
    }

    private static void OnLocalizationServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uc = (InnerPresetCategoriesFilterControl) d;
        if(uc.DataContext is DynamicCategoriesFilterViewModel vm) {
            var localizationService = (ILocalizationService) e.NewValue;
            vm.LocalizationProvider.OutsourceLocalization = localizationService;
        }
    }

    private static void OnLanguageServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uc = (InnerPresetCategoriesFilterControl) d;
        if(uc.DataContext is DynamicCategoriesFilterViewModel vm) {
            var languageService = (ILanguageService) e.NewValue;
            if(languageService != null) {
                vm.LocalizationProvider.SetInnerLocalization(languageService.HostLanguage);
            }
        }
    }
}
