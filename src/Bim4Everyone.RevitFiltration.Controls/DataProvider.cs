using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Providers;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;

using dosymep.Bim4Everyone;

namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Провайдер данных для построения фильтра по категориям: доступные категории, параметры и значения параметров.
/// </summary>
public sealed class DataProvider {
    private readonly ICategoriesProvider _categoriesProvider;
    private readonly IParamsProvider _paramsProvider;
    private readonly IParamValuesProvider _paramValuesProvider;

    /// <summary>
    ///     Создает провайдер, который берет значения параметров из экземпляров элементов заданных документов.
    /// </summary>
    /// <param name="categories">Доступные для фильтрации категории.</param>
    /// <param name="getParams">Функция получения доступных параметров для заданных категорий.</param>
    /// <param name="documents">Документы, из которых берутся существующие значения параметров для выпадающего списка.</param>
    public DataProvider(
        ICollection<Category> categories,
        Func<ICollection<Category>, ICollection<RevitParam>> getParams,
        ICollection<Document> documents) {
        _categoriesProvider = new CategoriesProvider(categories);
        _paramsProvider = new ParamsProvider(getParams);
        _paramValuesProvider = new InstancesParamValuesProvider(documents);
    }

    /// <summary>
    ///     Создает провайдер, который берет значения параметров из пользовательской функции
    ///     (позволяет переопределить список существующих значений параметра).
    /// </summary>
    /// <param name="categories">Доступные для фильтрации категории.</param>
    /// <param name="getParams">Функция получения доступных параметров для заданных категорий.</param>
    /// <param name="getParamValues">Функция получения существующих значений параметра для выпадающего списка.</param>
    public DataProvider(
        ICollection<Category> categories,
        Func<ICollection<Category>, ICollection<RevitParam>> getParams,
        Func<ICollection<Category>, RevitParam, ICollection<string>> getParamValues) {
        _categoriesProvider = new CategoriesProvider(categories);
        _paramsProvider = new ParamsProvider(getParams);
        _paramValuesProvider = new CustomParamValuesProvider(getParamValues);
    }

    internal ICollection<Category> GetCategories() {
        return _categoriesProvider.GetCategories();
    }

    internal ICollection<RevitParam> GetParams(ICollection<Category> categories) {
        return _paramsProvider.GetParams(categories);
    }

    internal ICollection<ParamValue> GetParamValues(ICollection<Category> categories, ParamModel param) {
        return _paramValuesProvider.GetParamValues(categories, param);
    }
}
