using System.Globalization;

using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Compositors;
using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;
using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Operators;
using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

using dosymep.Bim4Everyone;

namespace Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

/// <summary>
///     Преобразует <see cref="ILogicalFilter" /> в модель фильтра <see cref="Set" />, использующуюся в UI.
///     Операция, обратная методу <see cref="Set.Generate" />.
/// </summary>
internal class LogicalFilterConverter {
    private readonly Dictionary<Type, OperatorKind> _operatorKinds = new() {
        { typeof(EqualsOperator), OperatorKind.Equals },
        { typeof(NotEqualsOperator), OperatorKind.NotEquals },
        { typeof(HasValueOperator), OperatorKind.HasValue },
        { typeof(HasNoValueOperator), OperatorKind.HasNoValue },
        { typeof(GreaterOperator), OperatorKind.Greater },
        { typeof(GreaterOrEqualOperator), OperatorKind.GreaterOrEqual },
        { typeof(LessOperator), OperatorKind.Less },
        { typeof(LessOrEqualOperator), OperatorKind.LessOrEqual },
        { typeof(BeginsWithOperator), OperatorKind.BeginsWith },
        { typeof(NotBeginsWithOperator), OperatorKind.NotBeginsWith },
        { typeof(ContainsOperator), OperatorKind.Contains },
        { typeof(NotContainsOperator), OperatorKind.NotContains },
        { typeof(EndsWithOperator), OperatorKind.EndsWith },
        { typeof(NotEndsWithOperator), OperatorKind.NotEndsWith }
    };

    /// <summary>
    ///     Пытается преобразовать <see cref="ILogicalFilter" /> в <see cref="Set" />,
    ///     сопоставляя параметры правил с доступными для выбранных категорий.
    ///     Если в каком-то правиле из <paramref name="filterToConvert" /> используется параметр, которого нет в
    ///     <paramref name="availableParams" />, то возвращается False.
    /// </summary>
    /// <param name="filterToConvert">Фильтр для преобразования.</param>
    /// <param name="availableParams">Параметры, доступные для использования.</param>
    /// <param name="result">Модель набор правил, использующаяся в UI.</param>
    /// <returns>True, если весь фильтр удалось преобразовать, иначе - False.</returns>
    public bool TryConvert(ILogicalFilter filterToConvert, ICollection<RevitParam> availableParams, out Set? result) {
        var paramModels = availableParams
            .Where(p => !string.IsNullOrWhiteSpace(p.Id))
            .Select(p => new ParamModel(p))
            .ToArray();
        return TryConvert(filterToConvert, paramModels, out result);
    }

    private bool TryConvert(ILogicalFilter filterToConvert, ICollection<ParamModel> availableParams, out Set? result) {
        result = null;
        if(filterToConvert is not LogicalFilter logicalFilter
           || !TryConvertCompositor(logicalFilter.Compositor, out var compositorKind)) {
            return false;
        }

        List<Rule> innerRules = [];
        foreach(var rule in logicalFilter.InnerRules) {
            if(!TryConvertRule(rule, availableParams, out var innerRule)) {
                return false;
            }

            innerRules.Add(innerRule!);
        }

        List<Set> innerSets = [];
        foreach(var innerFilter in logicalFilter.InnerFilters) {
            if(!TryConvert(innerFilter, availableParams, out var innerSet)) {
                return false;
            }

            innerSets.Add(innerSet!);
        }

        result = new Set {
            CompositorKind = compositorKind,
            InnerRules = innerRules,
            InnerSets = innerSets
        };
        return true;
    }

    private bool TryConvertRule(
        Filtration.Rule ruleToConvert,
        ICollection<ParamModel> availableParams,
        out Rule? result) {
        result = null;
        if(!TryConvertParam(ruleToConvert.Param, availableParams, out var paramModel)
           || !TryConvertOperator(ruleToConvert.OperatorToken.Operator, out var operatorKind)
           || !paramModel!.GetOperatorKinds().Contains(operatorKind)
           || !TryGetParamValue(ruleToConvert.OperatorToken, paramModel, operatorKind, out var value)) {
            return false;
        }

        result = new Rule {
            OperatorKind = operatorKind,
            Param = paramModel,
            Value = value!
        };
        return true;
    }

    private bool TryConvertCompositor(ICompositor compositorToConvert, out CompositorKind result) {
        switch(compositorToConvert) {
            case AndCompositor:
                result = CompositorKind.And;
                return true;
            case OrCompositor:
                result = CompositorKind.Or;
                return true;
            default:
                result = default;
                return false;
        }
    }

    private bool TryConvertParam(
        IParam paramToConvert,
        ICollection<ParamModel> availableParams,
        out ParamModel? result) {
        result = paramToConvert switch {
            BuiltInParam builtInParam => availableParams.FirstOrDefault(p =>
                p.IsSystemParam(out var availableSysParam) && availableSysParam == builtInParam.Param),
            UserParam userParam => availableParams.FirstOrDefault(p =>
                !p.IsSystemParam(out _) && p.Name == userParam.Name),
            _ => null
        };
        return result != null;
    }

    private bool TryConvertOperator(IOperator operatorToConvert, out OperatorKind result) {
        return _operatorKinds.TryGetValue(operatorToConvert.GetType(), out result);
    }

    private bool TryGetParamValue(
        IOperatorToken operatorToken,
        ParamModel paramModel,
        OperatorKind operatorKind,
        out ParamValue? value) {
        value = null;
        if(operatorKind is OperatorKind.HasValue or OperatorKind.HasNoValue) {
            if(operatorToken is not EmptyOperatorToken) {
                return false;
            }

            value = new StringParamValue(string.Empty, string.Empty);
            return true;
        }

        switch(operatorToken) {
            case IntOperatorToken intToken when paramModel.StorageType == StorageType.Integer:
                value = new IntParamValue(intToken.Value, intToken.Value.ToString(CultureInfo.InvariantCulture));
                return true;
            case DoubleOperatorToken doubleToken when paramModel.StorageType == StorageType.Double:
                value = paramModel.GetParamValueFromDouble(doubleToken.Value);
                return true;
            case StringOperatorToken stringToken when !string.IsNullOrWhiteSpace(stringToken.Value):
                return TryGetStringParamValue(stringToken.Value, paramModel, out value);
            case ElementIdOperatorToken:
                // ElementIdOperatorToken в UI не представим: значение хранится строкой
                return false;
            default:
                return false;
        }
    }

    private bool TryGetStringParamValue(string value, ParamModel paramModel, out ParamValue? paramValue) {
        paramValue = paramModel.StorageType switch {
            StorageType.String => new StringParamValue(value, value),
            StorageType.ElementId => new ElementIdParamValue(value, value),
            // у рабочего набора StorageType - Integer, но значение используется строковое
            StorageType.Integer when paramModel.IsElemPartitionParam() => new StringParamValue(value, value),
            _ => null
        };
        return paramValue != null;
    }
}
