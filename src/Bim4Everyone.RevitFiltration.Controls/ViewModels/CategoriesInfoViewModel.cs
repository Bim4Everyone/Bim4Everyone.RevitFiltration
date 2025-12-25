using System.Collections.ObjectModel;

using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.Params;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class CategoriesInfoViewModel : BaseViewModel {
    private readonly ObservableCollection<ParamViewModel> _availableParams;
    private readonly IParamsProvider _paramsProvider;
    private readonly ObservableCollection<CategoryViewModel> _selectedCategories;

    public CategoriesInfoViewModel(IParamsProvider paramsProvider, ICollection<Category> selectedCategories) {
        if(selectedCategories == null) {
            throw new ArgumentNullException(nameof(selectedCategories));
        }

        _paramsProvider = paramsProvider ?? throw new ArgumentNullException(nameof(paramsProvider));
        _selectedCategories = [];
        _availableParams = [];
        SelectedCategories = new ReadOnlyObservableCollection<CategoryViewModel>(_selectedCategories);
        AvailableParams = new ReadOnlyObservableCollection<ParamViewModel>(_availableParams);

        SetSelectedCategories(selectedCategories);
    }

    public ReadOnlyObservableCollection<CategoryViewModel> SelectedCategories { get; }

    public ReadOnlyObservableCollection<ParamViewModel> AvailableParams { get; }

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
        var @params = _paramsProvider.GetParams(categories)
            .Select(p => new ParamViewModel(new ParamModel(p)))
            .Distinct()
            .OrderBy(p => p.Name)
            .ToArray();
        foreach(var param in @params) {
            _availableParams.Add(param);
        }
    }
}
