using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Compositors;
using Bim4Everyone.RevitFiltration.Operators;
using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Filtration;

internal class LogicalFilter : ILogicalFilter {
    [JsonConstructor]
    public LogicalFilter(ICompositor compositor) {
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

    /// <inheritdoc />
    public ElementFilter Build(Document document, Options options) {
        if(document == null) {
            throw new ArgumentNullException(nameof(document));
        }

        if(options == null) {
            throw new ArgumentNullException(nameof(options));
        }

        return Build(document, options, true);
    }

    private ElementFilter Build(Document document, Options options, bool isRoot) {
        List<ElementFilter> filters = [];
        filters.AddRange(
            InnerRules
                .Select(r => new ElementParameterFilter(r.CreateFilterRule(document, options), false)));
        filters.AddRange(
            InnerFilters
                .Select(s => s is LogicalFilter lf
                    ? lf.Build(document, options, false)
                    : s.Build(document, options)));
        if(filters.Count == 0) {
            // пустой набор не инвертируется: фильтр по всем экземплярам либо по всем типоразмерам
            return new ElementIsElementTypeFilter(!options.FilterByType);
        }

        var combined = options.Inverted
            ? Compositor.Invert().Create(filters)
            : Compositor.Create(filters);
        if(isRoot && options.FilterByType) {
            // ограничиваем результат типоразмерами (типами элементов)
            return new LogicalAndFilter(combined, new ElementIsElementTypeFilter(false));
        }

        return combined;
    }

    /// <inheritdoc />
    public ILogicalFilter AddFilter(ILogicalFilter innerFilter) {
        if(innerFilter == null) {
            throw new ArgumentNullException(nameof(innerFilter));
        }

        if(innerFilter is LogicalFilter filter
           && IsFilterInside(this, filter)) {
            throw new ArgumentException(
                "Нельзя добавить фильтр внутрь вложенного в него фильтра.",
                nameof(innerFilter));
        }

        InnerFilters.Add(innerFilter);
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterOrEqualRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new IntOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterOrEqualRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new DoubleOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterOrEqualRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterOrEqualRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new ElementIdOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new GreaterOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new IntOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new DoubleOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new ElementIdOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddGreaterRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new GreaterOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessOrEqualRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new IntOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessOrEqualRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new DoubleOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessOrEqualRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessOrEqualRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new ElementIdOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new LessOrEqualOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new IntOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new DoubleOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new ElementIdOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddLessRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new LessOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEqualsRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new IntOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEqualsRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new DoubleOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEqualsRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEqualsRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new ElementIdOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEqualsRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new EqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEqualsRule(string paramName, int paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new IntOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEqualsRule(string paramName, double paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new DoubleOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEqualsRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEqualsRule(string paramName, ElementId paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new ElementIdOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, int paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new IntOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, double paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new DoubleOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, ElementId paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new ElementIdOperatorToken(new NotEqualsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddHasNoValueRule(string paramName) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new EmptyOperatorToken(new HasNoValueOperator())));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddHasNoValueRule(BuiltInParameter paramId) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new EmptyOperatorToken(new HasNoValueOperator())));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddHasValueRule(string paramName) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new EmptyOperatorToken(new HasValueOperator())));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddHasValueRule(BuiltInParameter paramId) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new EmptyOperatorToken(new HasValueOperator())));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddBeginsWithRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new BeginsWithOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddBeginsWithRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new BeginsWithOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotBeginsWithRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new NotBeginsWithOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotBeginsWithRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new NotBeginsWithOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEndsWithRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new EndsWithOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddEndsWithRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new EndsWithOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEndsWithRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new NotEndsWithOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotEndsWithRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new NotEndsWithOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddContainsRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new ContainsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddContainsRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new ContainsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotContainsRule(string paramName, string paramValue) {
        if(string.IsNullOrWhiteSpace(paramName)) {
            throw new ArgumentException("Название параметра пустая строка или null.", nameof(paramName));
        }

        InnerRules.Add(
            new Rule(
                new UserParam(paramName),
                new StringOperatorToken(new NotContainsOperator(), paramValue)));
        return this;
    }

    /// <inheritdoc />
    public ILogicalFilter AddNotContainsRule(BuiltInParameter paramId, string paramValue) {
        InnerRules.Add(
            new Rule(
                new BuiltInParam(paramId),
                new StringOperatorToken(new NotContainsOperator(), paramValue)));
        return this;
    }

    /// <summary>
    ///     Проверяет является ли <paramref name="innerLogicalFilter" />
    ///     вложенным в <paramref name="parentLogicalFilter" />.
    /// </summary>
    /// <param name="innerLogicalFilter">Вложенный фильтр</param>
    /// <param name="parentLogicalFilter">Родительский фильтр</param>
    /// <returns>
    ///     True если <see cref="InnerFilters" /> содержит <paramref name="innerLogicalFilter" />
    ///     или если <paramref name="innerLogicalFilter" /> является <paramref name="parentLogicalFilter" />.
    /// </returns>
    private bool IsFilterInside(LogicalFilter innerLogicalFilter, LogicalFilter parentLogicalFilter) {
        if(ReferenceEquals(parentLogicalFilter, innerLogicalFilter)) {
            return true;
        }

        if(parentLogicalFilter.InnerFilters.Count == 0) {
            return false;
        }

        if(parentLogicalFilter.InnerFilters.Any(s => ReferenceEquals(s, innerLogicalFilter))) {
            return true;
        }

        foreach(var s in parentLogicalFilter.InnerFilters) {
            if(s is LogicalFilter filter
               && IsFilterInside(innerLogicalFilter, filter)) {
                return true;
            }
        }

        return false;
    }
}
