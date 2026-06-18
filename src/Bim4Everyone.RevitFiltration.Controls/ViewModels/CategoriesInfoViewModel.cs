using System.Collections.ObjectModel;

using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Services;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class CategoriesInfoViewModel : BaseViewModel {
    private readonly ObservableCollection<ParamViewModel> _availableParams;
    private readonly DataProvider _dataProvider;
    private readonly ILocalizationProvider _localization;
    private readonly ObservableCollection<CategoryViewModel> _selectedCategories;

    public CategoriesInfoViewModel(
        ILocalizationProvider localization,
        DataProvider dataProvider,
        ICollection<Category> selectedCategories) {
        if(selectedCategories == null) {
            throw new ArgumentNullException(nameof(selectedCategories));
        }

        _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        _selectedCategories = [];
        _availableParams = [];
        SelectedCategories = new ReadOnlyObservableCollection<CategoryViewModel>(_selectedCategories);
        AvailableParams = new ReadOnlyObservableCollection<ParamViewModel>(_availableParams);

        SetSelectedCategories(selectedCategories);
    }

    public ReadOnlyObservableCollection<CategoryViewModel> SelectedCategories { get; }

    public ReadOnlyObservableCollection<ParamViewModel> AvailableParams { get; }

    public ICollection<ParamValueViewModel> GetValues(
        ICollection<CategoryViewModel> categories,
        ParamViewModel param,
        OperatorKind @operator) {
        if(@operator is OperatorKind.HasValue or OperatorKind.HasNoValue) {
            return [];
        }

        return _dataProvider.GetParamValues(
                categories.Select(c => c.Category).ToArray(),
                param.ParamModel)
            .OrderBy(v => v)
            .Select(v => new ParamValueViewModel(v))
            .ToArray();
    }

    public void SetSelectedCategories(ICollection<Category> categories) {
        if(categories == null) {
            throw new ArgumentNullException(nameof(categories));
        }

        _selectedCategories.Clear();
        foreach(var category in categories) {
            _selectedCategories.Add(new CategoryViewModel(category));
        }

        SetParams(categories);
    }

    private void SetParams(ICollection<Category> categories) {
        _availableParams.Clear();
        var @params = _dataProvider.GetParams(categories)
            .Select(p => new ParamViewModel(_localization, new ParamModel(p)))
            .Distinct()
            .OrderBy(p => p.Name)
            .ToArray();
        foreach(var param in @params) {
            _availableParams.Add(param);
        }
    }
}
