using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models;

internal class LogicalFilterProvider : ILogicalFilterProvider {
    private readonly IDataProvider _dataProvider;
    private readonly List<IErrorContext> _errors;
    private readonly ILogicalFilterFactory _factory;
    private ILogicalFilterContext? _filterContext;

    public LogicalFilterProvider(
        IDataProvider dataProvider,
        ILogicalFilterFactory factory) {
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _errors = [new ErrorContext("Ничего не выбрано")];
        _filterContext = null;
    }

    public LogicalFilterProvider(
        IDataProvider dataProvider,
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

    public IDataProvider GetDataProvider() {
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

        _errors.Clear();
        _errors.AddRange(errors);
        _filterContext = null;
    }

    public void SetFilter(ILogicalFilterContext filter) {
        _filterContext = filter ?? throw new ArgumentNullException(nameof(filter));
        _errors.Clear();
    }

    public ICollection<Category> GetAvailableCategories() {
        return _dataProvider.GetCategories();
    }

    public IDataProvider GetParamsProvider() {
        return _dataProvider;
    }

    private void Load(ILogicalFilterContext filterContext) {
        throw new NotImplementedException();
    }
}
