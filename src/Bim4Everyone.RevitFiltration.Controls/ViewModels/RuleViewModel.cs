using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class RuleViewModel : BaseViewModel {
    private static readonly IReadOnlyDictionary<OperatorKind, OperatorViewModel> _allOperators =
        new ReadOnlyDictionary<OperatorKind, OperatorViewModel>(
            new Dictionary<OperatorKind, OperatorViewModel> {
                { OperatorKind.Equals, new OperatorViewModel(OperatorKind.Equals) },
                { OperatorKind.NotEquals, new OperatorViewModel(OperatorKind.NotEquals) },
                { OperatorKind.HasValue, new OperatorViewModel(OperatorKind.HasValue) },
                { OperatorKind.HasNoValue, new OperatorViewModel(OperatorKind.HasNoValue) },
                { OperatorKind.Greater, new OperatorViewModel(OperatorKind.Greater) },
                { OperatorKind.GreaterOrEqual, new OperatorViewModel(OperatorKind.GreaterOrEqual) },
                { OperatorKind.Less, new OperatorViewModel(OperatorKind.Less) },
                { OperatorKind.LessOrEqual, new OperatorViewModel(OperatorKind.LessOrEqual) },
                { OperatorKind.BeginsWith, new OperatorViewModel(OperatorKind.BeginsWith) },
                { OperatorKind.NotBeginsWith, new OperatorViewModel(OperatorKind.NotBeginsWith) },
                { OperatorKind.Contains, new OperatorViewModel(OperatorKind.Contains) },
                { OperatorKind.NotContains, new OperatorViewModel(OperatorKind.NotContains) },
                { OperatorKind.EndsWith, new OperatorViewModel(OperatorKind.EndsWith) },
                { OperatorKind.NotEndsWith, new OperatorViewModel(OperatorKind.NotEndsWith) }
            }
        );

    private bool _isValueEditable;

    /// <summary>
    ///     Флаг, показывающий, обновлен ли список существующих значений выбранного параметра
    /// </summary>
    private bool _paramValuesAlreadyUpdated;

    private OperatorViewModel? _selectedOperator;
    private ParamViewModel? _selectedParameter;
    private ParamValueViewModel? _selectedValue;
    private string _stringValue;
    private ObservableCollection<ParamValueViewModel>? _values;

    public RuleViewModel(CategoriesInfoViewModel categoriesInfo, Rule? rule = null) {
        CategoriesInfo = categoriesInfo ?? throw new ArgumentNullException(nameof(categoriesInfo));

        if(rule != null) {
            SelectedParameter = new ParamViewModel(rule.Param);
            SelectedOperator = new OperatorViewModel(rule.OperatorKind);
            AvailableOperators = [..SelectedParameter.ParamModel.GetOperatorKinds().Select(o => _allOperators[o])];
            StringValue = rule.Value.DisplayValue ?? string.Empty;
        } else {
            SelectedParameter = CategoriesInfo.AvailableParams.First();
            AvailableOperators = [..SelectedParameter.ParamModel.GetOperatorKinds().Select(o => _allOperators[o])];
            SelectedOperator = AvailableOperators.First();
            StringValue = string.Empty;
        }

        OnSelectedOperatorChanged();

        PropertyChanged += RuleViewModelChangedHandler;

        UpdateParamValuesCommand = RelayCommand.Create(UpdateParamValues, CanUpdateParamValues);
    }

    /// <summary>
    ///     Обновляет список существующих значений параметра для выбора в выпадающем списке
    /// </summary>
    public ICommand UpdateParamValuesCommand { get; }

    public ObservableCollection<OperatorViewModel> AvailableOperators { get; } = [];

    public bool IsValueEditable {
        get => _isValueEditable;
        private set => RaiseAndSetIfChanged(ref _isValueEditable, value);
    }

    public string StringValue {
        get => _stringValue;
        set => RaiseAndSetIfChanged(ref _stringValue, value);
    }

    public ParamValueViewModel? SelectedValue {
        get => _selectedValue;
        set => RaiseAndSetIfChanged(ref _selectedValue, value);
    }

    public OperatorViewModel? SelectedOperator {
        get => _selectedOperator;
        set => RaiseAndSetIfChanged(ref _selectedOperator, value);
    }

    public ParamViewModel? SelectedParameter {
        get => _selectedParameter;
        set => RaiseAndSetIfChanged(ref _selectedParameter, value);
    }

    public ObservableCollection<ParamValueViewModel>? Values {
        get => _values;
        private set => RaiseAndSetIfChanged(ref _values, value);
    }

    public CategoriesInfoViewModel CategoriesInfo { get; }

    /// <summary>
    ///     Возвращает модель правила фильтрации для UI.
    ///     Перед вызовом этого метода надо проверить, что в текущем правиле нет ошибок и оно не пустое.
    /// </summary>
    public Rule CreateRule() {
        if(!string.IsNullOrWhiteSpace(GetErrorText())) {
            throw new InvalidOperationException();
        }

        ParamValue value;
        if(SelectedOperator!.Operator is OperatorKind.HasValue or OperatorKind.HasNoValue) {
            value = new StringParamValue(string.Empty, string.Empty);
        } else if(SelectedValue == null
                  || SelectedValue.DisplayValue != StringValue) {
            value = SelectedParameter!.ParamModel.GetParamValueFromString(StringValue);
        } else {
            value = SelectedValue.ParamValue;
        }

        return new Rule {
            OperatorKind = SelectedOperator.Operator,
            Param = SelectedParameter!.ParamModel,
            Value = value
        };
    }

    public bool IsEmpty() {
        return SelectedParameter == null
               || SelectedOperator == null
               || (SelectedOperator.Operator is not OperatorKind.HasNoValue and not OperatorKind.HasValue
                   && string.IsNullOrWhiteSpace(StringValue)
                   && SelectedValue == null);
    }

    public string GetErrorText() {
        if(SelectedParameter is null) {
            return "Не выбран параметр у правила";
        }

        if(SelectedOperator is null) {
            return "Не выбран оператор у правила";
        }

        return SelectedOperator.Operator is OperatorKind.HasValue or OperatorKind.HasNoValue
            ? string.Empty
            : SelectedParameter.GetErrorText(StringValue);
    }

    public void Renew() {
        SelectedParameter = null;
        SelectedOperator = null;
        StringValue = string.Empty;
        SelectedValue = null;
    }

    private void RuleViewModelChangedHandler(object sender, PropertyChangedEventArgs e) {
        if(e.PropertyName == nameof(SelectedParameter)) {
            PropertyChanged -= RuleViewModelChangedHandler;
            OnSelectedParamChanged();
            PropertyChanged += RuleViewModelChangedHandler;
        }

        if(e.PropertyName == nameof(SelectedOperator)) {
            PropertyChanged -= RuleViewModelChangedHandler;
            OnSelectedOperatorChanged();
            PropertyChanged += RuleViewModelChangedHandler;
        }
    }

    private void OnSelectedParamChanged() {
        if(SelectedParameter != null) {
            AvailableOperators.Clear();
            var availableOperators = SelectedParameter.ParamModel.GetOperatorKinds().Select(o => _allOperators[o]);
            foreach(var op in availableOperators) {
                AvailableOperators.Add(op);
            }

            SelectedOperator = AvailableOperators.First();
            OnSelectedOperatorChanged();
            _paramValuesAlreadyUpdated = false;
        }
    }

    private void OnSelectedOperatorChanged() {
        if(SelectedParameter != null
           && SelectedOperator != null) {
            IsValueEditable = SelectedOperator.Operator != OperatorKind.HasValue
                              && SelectedOperator.Operator != OperatorKind.HasNoValue;
            if(!IsValueEditable) {
                StringValue = string.Empty;
            }
        }
    }

    private void UpdateParamValues() {
        if(SelectedParameter == null
           || SelectedOperator == null) {
            return;
        }

        Values = [
            ..CategoriesInfo.GetValues(
                CategoriesInfo.SelectedCategories,
                SelectedParameter,
                SelectedOperator.Operator)
        ];
        _paramValuesAlreadyUpdated = true;
    }

    private bool CanUpdateParamValues() {
        return SelectedParameter != null && SelectedOperator != null && !_paramValuesAlreadyUpdated;
    }
}
