using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Compositors;
using Bim4Everyone.RevitFiltration.Operators;
using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

namespace Bim4Everyone.RevitFiltration.Filtration;

internal class Set : ILogicalFilter {
    private readonly List<ILogicalFilter> _innerFilters;
    private readonly List<Rule> _innerRules;
    private readonly IOptions _options;

    public Set(ICompositor compositor, IOptions options) {
        Compositor = compositor ?? throw new ArgumentNullException(nameof(compositor));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _innerRules = [];
        _innerFilters = [];
    }

    public ICompositor Compositor { get; }

    public ElementFilter Build(Document document, IOptions options) {
        List<ElementFilter> filters = [];
        filters.AddRange(
            _innerRules
                .Select(r => new ElementParameterFilter(r.CreateFilterRule(document), false)));
        filters.AddRange(
            _innerFilters
                .Select(s => s.Build(document, options)));
        return Compositor.Create(filters);
    }

    public ILogicalFilter AddFilter(ILogicalFilter innerFilter) {
        if(innerFilter == null) {
            throw new ArgumentNullException(nameof(innerFilter));
        }

        if(innerFilter is Set set
           && IsSetInside(this, set)) {
            throw new ArgumentException("Нельзя рекурсивно добавить фильтр.", nameof(innerFilter));
        }

        _innerFilters.Add(innerFilter);
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        _innerRules.Add(new Rule(new NamedParam(paramName), new IntOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(string paramName, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterOrEqualRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterOrEqualRule(string paramName, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterRule(string paramName, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterRule(string paramName, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterRule(string paramName, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessOrEqualRule(string paramName, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessOrEqualRule(string paramName, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessOrEqualRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessOrEqualRule(string paramName, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessRule(string paramName, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessRule(string paramName, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessRule(string paramName, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessRule(BuiltInParameter paramId, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessRule(BuiltInParameter paramId, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddLessRule(BuiltInParameter paramId, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEqualsRule(string paramName, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEqualsRule(string paramName, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEqualsRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEqualsRule(string paramName, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEqualsRule(string paramName, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEqualsRule(string paramName, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEqualsRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEqualsRule(string paramName, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, int paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, double paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, ElementId paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddHasNoValueRule(string paramName) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddHasNoValueRule(BuiltInParameter paramId) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddHasValueRule(string paramName) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddHasValueRule(BuiltInParameter paramId) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddBeginsWithRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddBeginsWithRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotBeginsWithRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotBeginsWithRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEndsWithRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddEndsWithRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEndsWithRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotEndsWithRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddContainsRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddContainsRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotContainsRule(string paramName, string paramValue) {
        throw new NotImplementedException();
    }

    public ILogicalFilter AddNotContainsRule(BuiltInParameter paramId, string paramValue) {
        throw new NotImplementedException();
    }

    private bool IsSetInside(Set innerSet, Set parentSet) {
        if(ReferenceEquals(parentSet, innerSet)) {
            return true;
        }

        if(parentSet._innerFilters.Count == 0) {
            return false;
        }

        if(parentSet._innerFilters.Any(s => ReferenceEquals(s, innerSet))) {
            return true;
        }

        foreach(var s in _innerFilters) {
            if(s is Set set
               && IsSetInside(innerSet, set)) {
                return true;
            }
        }

        return false;
    }
}
