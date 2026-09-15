using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Compositors;
using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Operators;
using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

using dosymep.Revit;

namespace Bim4Everyone.RevitFiltration.Extensions.Materials;

/// <summary>
///     Проверяет материал на соответствие правилам <see cref="ILogicalFilter" /> без создания фильтров Revit.
///     Повторяет структуру <see cref="LogicalFilter.Build(Document, Options)" />, но вместо сборки
///     <see cref="T:Autodesk.Revit.DB.ElementFilter" /> вычисляет логическое значение.
/// </summary>
internal class MaterialsFilterEvaluator {
    private readonly Document _document;
    private readonly MaterialOptions _options;
    private readonly LogicalFilter _rootFilter;

    public MaterialsFilterEvaluator(Document document, LogicalFilter rootFilter, MaterialOptions options) {
        _document = document ?? throw new ArgumentNullException(nameof(document));
        _rootFilter = rootFilter ?? throw new ArgumentNullException(nameof(rootFilter));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    ///     Проверяет, проходит ли материал правила фильтра.
    /// </summary>
    /// <param name="material">Проверяемый материал.</param>
    /// <returns>True, если материал проходит фильтр, иначе - False.</returns>
    public bool Evaluate(Element material) {
        return Evaluate(_rootFilter, material);
    }

    private bool Evaluate(ILogicalFilter filter, Element material) {
        if(filter is not LogicalFilter logicalFilter) {
            throw new InvalidOperationException();
        }

        List<bool> results = [];
        results.AddRange(logicalFilter.InnerRules.Select(r => EvaluateRule(r, material)));
        results.AddRange(logicalFilter.InnerFilters.Select(f => Evaluate(f, material)));
        if(results.Count == 0) {
            // пустой набор не инвертируется - как и в LogicalFilter.Build
            return true;
        }

        var compositor = _options.Inverted
            ? logicalFilter.Compositor.Invert()
            : logicalFilter.Compositor;
        return compositor is OrCompositor
            ? results.Any(r => r)
            : results.All(r => r);
    }

    private bool EvaluateRule(Rule rule, Element material) {
        if(!TryGetParameter(rule.Param, material, out var parameter)) {
            // у материала нет такого параметра - материал не подходит,
            // отсутствие параметра не инвертируется
            return false;
        }

        bool result = Matches(rule, parameter!);
        return _options.Inverted ? !result : result;
    }

    private bool Matches(Rule rule, Parameter parameter) {
        var @operator = rule.OperatorToken.Operator;
        return rule.OperatorToken switch {
            EmptyOperatorToken => EvaluateHasValue(@operator, parameter),
            IntOperatorToken token when parameter.StorageType == StorageType.Integer =>
                Compare(@operator, parameter.AsInteger().CompareTo(token.Value)),
            DoubleOperatorToken token when parameter.StorageType == StorageType.Double =>
                CompareDouble(@operator, parameter.AsDouble(), token.Value),
            ElementIdOperatorToken token when parameter.StorageType == StorageType.ElementId =>
                Compare(@operator, parameter.AsElementId().GetIdValue().CompareTo(token.Value.GetIdValue())),
            StringOperatorToken token => CompareString(@operator, GetStringValue(parameter), token.Value),
            _ => false
        };
    }

    /// <summary>
    ///     Возвращает параметр материала, если он у него есть.
    ///     Существование параметра проверяется до его получения: <c>GetParam</c>
    ///     на отсутствующем параметре выбрасывает исключение.
    /// </summary>
    /// <param name="param">Искомый параметр.</param>
    /// <param name="material">Проверяемый материал.</param>
    /// <param name="parameter">Параметр материала.</param>
    /// <returns>True, если параметр есть у материала, иначе - False.</returns>
    private bool TryGetParameter(IParam param, Element material, out Parameter? parameter) {
        parameter = null;
        var paramId = param.GetId(_document);
        if(paramId == ElementId.InvalidElementId) {
            // параметра нет в документе
            return false;
        }

        if(paramId.IsSystemId()) {
            var builtInParameter = paramId.AsBuiltInParameter();
            if(!material.IsExistsParam(builtInParameter)) {
                return false;
            }

            parameter = material.GetParam(builtInParameter);
            return true;
        }

        parameter = material.Parameters.OfType<Parameter>().FirstOrDefault(p => p.Id == paramId);
        return parameter != null;
    }

    /// <summary>
    ///     Возвращает строковое значение параметра
    /// </summary>
    private string GetStringValue(Parameter parameter) {
        return (parameter.StorageType == StorageType.String
                   ? parameter.AsString()
                   : parameter.AsValueString())
               ?? string.Empty;
    }

    private bool EvaluateHasValue(IOperator @operator, Parameter parameter) {
        return @operator switch {
            HasValueOperator => parameter.HasValue,
            HasNoValueOperator => !parameter.HasValue,
            _ => false
        };
    }

    private bool Compare(IOperator @operator, int comparison) {
        return @operator switch {
            EqualsOperator => comparison == 0,
            NotEqualsOperator => comparison != 0,
            GreaterOperator => comparison > 0,
            GreaterOrEqualOperator => comparison >= 0,
            LessOperator => comparison < 0,
            LessOrEqualOperator => comparison <= 0,
            _ => false
        };
    }

    private bool CompareDouble(IOperator @operator, double actual, double expected) {
        double tolerance = _options.Tolerance;
        return @operator switch {
            EqualsOperator => Math.Abs(actual - expected) <= tolerance,
            NotEqualsOperator => Math.Abs(actual - expected) > tolerance,
            GreaterOperator => (actual - expected) > tolerance,
            GreaterOrEqualOperator => (actual - expected) > -tolerance,
            LessOperator => (expected - actual) > tolerance,
            LessOrEqualOperator => (expected - actual) > -tolerance,
            _ => false
        };
    }

    /// <summary>
    ///     Сравнивает строки без учета регистра
    /// </summary>
    private bool CompareString(IOperator @operator, string actual, string expected) {
        const StringComparison comparison = StringComparison.CurrentCultureIgnoreCase;
        return @operator switch {
            ContainsOperator => actual.IndexOf(expected, comparison) >= 0,
            NotContainsOperator => actual.IndexOf(expected, comparison) < 0,
            BeginsWithOperator => actual.StartsWith(expected, comparison),
            NotBeginsWithOperator => !actual.StartsWith(expected, comparison),
            EndsWithOperator => actual.EndsWith(expected, comparison),
            NotEndsWithOperator => !actual.EndsWith(expected, comparison),
            _ => Compare(@operator, string.Compare(actual, expected, comparison))
        };
    }
}
