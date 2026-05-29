using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Models.Params;

using dosymep.Bim4Everyone;
using dosymep.Revit;

namespace Bim4Everyone.RevitFiltration.Controls.Models;

internal class LogicalFilterProvider : ILogicalFilterProvider {
    private readonly DataProvider _dataProvider;
    private readonly List<IErrorContext> _errors;
    private readonly ILogicalFilterFactory _factory;
    private ILogicalFilterContext? _filterContext;

    public LogicalFilterProvider(
        DataProvider dataProvider,
        ILogicalFilterFactory factory) {
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _errors = [];
        _filterContext = null;
    }

    public LogicalFilterProvider(
        DataProvider dataProvider,
        ILogicalFilterFactory factory,
        ILogicalFilterContext filterContext) {
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        if(filterContext is null) {
            throw new ArgumentNullException(nameof(filterContext));
        }

        _errors = [];
        Load(filterContext);
    }

    public event EventHandler<FilterContextChangedEventArgs>? FilterContextChanged;

    public ILogicalFilterContext GetFilter() {
        if(_filterContext is null) {
            throw new InvalidOperationException();
        }

        return _filterContext;
    }

    public bool CanGetFilter(out IErrorContext[] errors) {
        errors = _errors.ToArray();
        return _errors.Count == 0 && _filterContext is not null;
    }

    public DataProvider GetDataProvider() {
        return _dataProvider;
    }

    public ILogicalFilterFactory GetLogicalFilterFactory() {
        return _factory;
    }

    public void SetErrors(IErrorContext[] errors) {
        if(errors == null) {
            throw new ArgumentNullException(nameof(errors));
        }

        if(errors.Length == 0) {
            throw new ArgumentException(nameof(errors));
        }

        var oldContext = _filterContext;
        _errors.Clear();
        _errors.AddRange(errors);
        _filterContext = null;
        RaiseFilterContextChanged(oldContext, null);
    }

    public void SetFilter(ILogicalFilterContext filter) {
        if(filter is null) {
            throw new ArgumentNullException(nameof(filter));
        }

        var oldContext = _filterContext;
        _filterContext = filter;
        _errors.Clear();
        RaiseFilterContextChanged(oldContext, _filterContext);
    }

    private void RaiseFilterContextChanged(
        ILogicalFilterContext? oldContext,
        ILogicalFilterContext? newContext) {
        if(ReferenceEquals(oldContext, newContext)) {
            return;
        }

        FilterContextChanged?.Invoke(this, new FilterContextChangedEventArgs(oldContext, newContext));
    }

    private void Load(ILogicalFilterContext filterContext) {
        if(filterContext == null) {
            throw new ArgumentNullException(nameof(filterContext));
        }

        var filter = filterContext.Filter;
        var availableCategories = _dataProvider.GetCategories()
            .ToDictionary(c => c.GetBuiltInCategory(), c => c);
        if(filter.Categories.Any(c => !availableCategories.ContainsKey(c))) {
            _errors.Clear();
            _filterContext = null;
            return;
        }

        var availableParams = _dataProvider.GetParams(
            filter.Categories.Select(c => availableCategories[c]).Distinct().ToArray());
        string[] availableParamIds = availableParams.Select(p => p.Id).ToArray();
        if(ContainsNotAvailableParams(filter.RootSet, availableParamIds)) {
            _errors.Clear();
            _filterContext = null;
            return;
        }

        BindRevitParams(filter.RootSet, availableParams);
        SetFilter(filterContext);
    }

    /// <summary>
    /// Задайт свойство <see cref="ParamModel.RevitParam"/>
    /// </summary>
    private void BindRevitParams(Set set, ICollection<RevitParam> availableParams) {
        foreach(var rule in set.InnerRules) {
            rule.Param.RevitParam = availableParams.FirstOrDefault(rp => rule.Param.Equals(rp));
        }

        foreach(var innerSet in set.InnerSets) {
            BindRevitParams(innerSet, availableParams);
        }
    }

    private bool ContainsNotAvailableParams(Set set, ICollection<string> paramIds) {
        if(set.InnerRules.Any(r => !paramIds.Contains(r.Param.Id))) {
            return true;
        }

        return set.InnerSets.Any(innerSet => ContainsNotAvailableParams(innerSet, paramIds));
    }
}
