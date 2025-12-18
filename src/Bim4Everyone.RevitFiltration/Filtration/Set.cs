using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Compositors;
using Bim4Everyone.RevitFiltration.Operators;
using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Filtration;

internal class Set : ILogicalFilter {
    [JsonConstructor]
    public Set(ICompositor compositor) {
        Compositor = compositor ?? throw new ArgumentNullException(nameof(compositor));
        InnerRules = [];
        InnerFilters = [];
    }

    [JsonProperty]
    public List<ILogicalFilter> InnerFilters { get; }

    [JsonProperty]
    public List<Rule> InnerRules { get; }

    [JsonProperty]
    public ICompositor Compositor { get; }

    public ElementFilter Build(Document document, IOptions options) {
        if(document == null) {
            throw new ArgumentNullException(nameof(document));
        }

        if(options == null) {
            throw new ArgumentNullException(nameof(options));
        }

        List<ElementFilter> filters = [];
        filters.AddRange(
            InnerRules
                .Select(r => new ElementParameterFilter(r.CreateFilterRule(document, options), false)));
        filters.AddRange(
            InnerFilters
                .Select(s => s.Build(document, options)));
        return Compositor.Create(filters);
    }

    public ILogicalFilter AddFilter(ILogicalFilter innerFilter) {
        if(innerFilter == null) {
            throw new ArgumentNullException(nameof(innerFilter));
        }

        if(innerFilter is Set set
           && IsSetInside(this, set)) {
            throw new ArgumentException(
                "Нельзя добавить фильтр внутрь вложенного в него фильтра.",
                nameof(innerFilter));
        }

        InnerFilters.Add(innerFilter);
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new IntOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new DoubleOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new ElementIdOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new IntOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new DoubleOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new ElementIdOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessOrEqualRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new IntOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessOrEqualRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new DoubleOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessOrEqualRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessOrEqualRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new ElementIdOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new IntOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new DoubleOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new ElementIdOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddLessRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEqualsRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new IntOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEqualsRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new DoubleOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEqualsRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEqualsRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new ElementIdOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEqualsRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new IntOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEqualsRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new DoubleOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEqualsRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEqualsRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new ElementIdOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddHasNoValueRule(string paramName) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new EmptyOperatorToken(new HasNoValueOperator())));
        return this;
    }

    public ILogicalFilter AddHasNoValueRule(BuiltInParameter paramId) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new EmptyOperatorToken(new HasNoValueOperator())));
        return this;
    }

    public ILogicalFilter AddHasValueRule(string paramName) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new EmptyOperatorToken(new HasValueOperator())));
        return this;
    }

    public ILogicalFilter AddHasValueRule(BuiltInParameter paramId) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new EmptyOperatorToken(new HasValueOperator())));
        return this;
    }

    public ILogicalFilter AddBeginsWithRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new BeginsWithOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddBeginsWithRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new BeginsWithOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotBeginsWithRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new NotBeginsWithOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotBeginsWithRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new NotBeginsWithOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEndsWithRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new EndsWithOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddEndsWithRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new EndsWithOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEndsWithRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new NotEndsWithOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotEndsWithRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new NotEndsWithOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddContainsRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new ContainsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddContainsRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new ContainsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotContainsRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new NamedParam(paramName),
                new StringOperatorToken(new NotContainsOperator(), paramValue)));
        return this;
    }

    public ILogicalFilter AddNotContainsRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new NotContainsOperator(), paramValue)));
        return this;
    }

    private bool IsSetInside(Set innerSet, Set parentSet) {
        if(ReferenceEquals(parentSet, innerSet)) {
            return true;
        }

        if(parentSet.InnerFilters.Count == 0) {
            return false;
        }

        if(parentSet.InnerFilters.Any(s => ReferenceEquals(s, innerSet))) {
            return true;
        }

        foreach(var s in InnerFilters) {
            if(s is Set set
               && IsSetInside(innerSet, set)) {
                return true;
            }
        }

        return false;
    }
}
