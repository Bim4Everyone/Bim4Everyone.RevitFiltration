using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Models.Utils;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class DynamicCategoriesFilterViewModel : BaseViewModel {
    private readonly Delay _delay;
    private ObservableCollection<CategoryViewModel>? _allCategories;
    private bool _allCategoriesSelected;
    private CollectionViewSource? _categories;
    private string _categoriesFilter;
    private CategoriesInfoViewModel? _categoriesInfo;
    private bool _hideUnselectedCategories;
    private ILogicalFilterProvider? _logicalFilterProvider;
    private SetViewModel? _rootSet;

    public DynamicCategoriesFilterViewModel() {
        _delay = new Delay(250, SelectedCategoriesChanged);
    }

    public SetViewModel? RootSet {
        get => _rootSet;
        private set => RaiseAndSetIfChanged(ref _rootSet, value);
    }

    public CategoriesInfoViewModel? CategoriesInfo {
        get => _categoriesInfo;
        private set => RaiseAndSetIfChanged(ref _categoriesInfo, value);
    }

    public ObservableCollection<CategoryViewModel>? AllCategories {
        get => _allCategories;
        private set => RaiseAndSetIfChanged(ref _allCategories, value);
    }

    public CollectionViewSource? Categories {
        get => _categories;
        private set => RaiseAndSetIfChanged(ref _categories, value);
    }

    public bool AllCategoriesSelected {
        get => _allCategoriesSelected;
        set {
            if(_allCategoriesSelected != value) {
                RaiseAndSetIfChanged(ref _allCategoriesSelected, value);
                if(AllCategories == null) {
                    return;
                }

                foreach(var category in AllCategories) {
                    category.IsSelected = value;
                }
            }
        }
    }

    public string CategoriesFilter {
        get => _categoriesFilter;
        set => RaiseAndSetIfChanged(ref _categoriesFilter, value);
    }

    public bool HideUnselectedCategories {
        get => _hideUnselectedCategories;
        set => RaiseAndSetIfChanged(ref _hideUnselectedCategories, value);
    }

    public void LoadProvider(ILogicalFilterProvider? provider) {
        _logicalFilterProvider = provider;
        UnsubscribeHandlers();
        AllCategoriesSelected = false;
        HideUnselectedCategories = false;

        if(provider == null) {
            return;
        }

        var dataProvider = provider.GetDataProvider();
        AllCategories = [..dataProvider.GetCategories().Select(c => new CategoryViewModel(c))];
        Categories = new CollectionViewSource { Source = AllCategories };
        Categories.Filter += CategoriesFilterHandler;
        var factory = provider.GetLogicalFilterFactory();
        if(provider.CanGetFilter(out _)) {
            var context = provider.GetFilter();
            CategoriesInfo = InitializeCategoriesInfo(dataProvider, context.SelectedCategories);
            RootSet = new SetViewModel(CategoriesInfo, factory, context.Filter.RootSet);
            foreach(var c in AllCategories.Intersect(CategoriesInfo.SelectedCategories)) {
                c.IsSelected = true;
            }
        } else {
            CategoriesInfo = InitializeCategoriesInfo(dataProvider);
            RootSet = new SetViewModel(CategoriesInfo, factory);
        }

        foreach(var category in AllCategories) {
            category.PropertyChanged += OnCategorySelectionChanged;
        }

        PropertyChanged += CategoriesFilterPropertyChanged;
        RootSet.PropertyChanged += RootSetChanged;
    }

    private void UnsubscribeHandlers() {
        if(AllCategories != null) {
            foreach(var category in AllCategories) {
                category.PropertyChanged -= OnCategorySelectionChanged;
            }
        }

        if(Categories != null) {
            Categories.Filter -= CategoriesFilterHandler;
        }

        PropertyChanged -= CategoriesFilterPropertyChanged;

        if(RootSet != null) {
            RootSet.PropertyChanged -= RootSetChanged;
        }
    }

    private void OnCategorySelectionChanged(object sender, PropertyChangedEventArgs e) {
        if(e.PropertyName.Equals(nameof(CategoryViewModel.IsSelected))) {
            _delay.Action();
        }
    }

    private void CategoriesFilterHandler(object sender, FilterEventArgs e) {
        if(e.Item is CategoryViewModel category) {
            if(HideUnselectedCategories && !category.IsSelected) {
                e.Accepted = false;
                return;
            }

            if(!string.IsNullOrWhiteSpace(CategoriesFilter)) {
                string str = CategoriesFilter.ToLower();
                e.Accepted = category.Name.IndexOf(str, StringComparison.CurrentCultureIgnoreCase) >= 0;
                return;
            }

            e.Accepted = true;
        }
    }

    private CategoriesInfoViewModel InitializeCategoriesInfo(
        IDataProvider provider,
        ICollection<BuiltInCategory>? selectedCategories = null) {
        if(selectedCategories == null
           || selectedCategories.Count == 0) {
            return new CategoriesInfoViewModel(provider, []);
        }

        return new CategoriesInfoViewModel(
            provider,
            provider.GetCategories()
                .Where(c => selectedCategories.Contains(c.GetBuiltInCategory()))
                .ToArray());
    }

    private void SelectedCategoriesChanged() {
        CategoriesInfo?.SetSelectedCategories(
            AllCategories?.Where(c => c.IsSelected)
                .Select(c => c.Category)
                .ToArray()
            ?? []);
        RootSet?.Renew();
        UpdateLogicalFilterContext();
    }

    private void RootSetChanged(object sender, PropertyChangedEventArgs e) {
        UpdateLogicalFilterContext();
    }

    public Filter GetFilter() {
        if(RootSet == null
           || CategoriesInfo == null) {
            throw new InvalidOperationException();
        }

        return new Filter {
            RootSet = RootSet.CreateSet(),
            Categories = CategoriesInfo.SelectedCategories.Select(c => c.Category.GetBuiltInCategory()).ToArray()
        };
    }

    private void UpdateLogicalFilterContext() {
        if(_logicalFilterProvider == null) {
            return;
        }

        var categories = CategoriesInfo?.SelectedCategories
                             .Select(c => c.Category.GetBuiltInCategory())
                             .ToArray()
                         ?? [];
        if(CategoriesInfo == null
           || categories.Length == 0) {
            _logicalFilterProvider.SetErrors([new ErrorContext("Необходимо выбрать категории")]);
            return;
        }

        if(RootSet == null) {
            _logicalFilterProvider.SetErrors([new ErrorContext("Критерии фильтрации отсутствуют")]);
            return;
        }

        if(RootSet.IsEmpty()) {
            _logicalFilterProvider.SetErrors([new ErrorContext("Критерии фильтрации пустые")]);
            return;
        }

        string error = RootSet.GetErrorText();
        if(!string.IsNullOrWhiteSpace(error)) {
            _logicalFilterProvider.SetErrors([new ErrorContext(error)]);
            return;
        }

        _logicalFilterProvider.SetFilter(
            new LogicalFilterContext(GetFilter()) { Factory = _logicalFilterProvider.GetLogicalFilterFactory() });
    }

    private void CategoriesFilterPropertyChanged(object sender, PropertyChangedEventArgs e) {
        if(e.PropertyName == nameof(CategoriesFilter)
           || e.PropertyName == nameof(HideUnselectedCategories)) {
            Categories?.View.Refresh();
        }
    }
}
