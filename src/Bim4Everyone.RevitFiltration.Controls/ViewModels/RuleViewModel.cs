using System.Collections.ObjectModel;
using System.ComponentModel;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

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
    private OperatorViewModel? _selectedOperator;
    private ParamViewModel? _selectedParameter;
    private string _stringValue;

    public RuleViewModel(CategoriesInfoViewModel categoriesInfo, Rule? rule = null) {
        CategoriesInfo = categoriesInfo ?? throw new ArgumentNullException(nameof(categoriesInfo));

        PropertyChanged += RuleViewModelChangedHandler;
        if(rule != null) {
            SelectedParameter = new ParamViewModel(rule.Param);
            StringValue = rule.Value.DisplayValue ?? string.Empty;
        } else {
            SelectedParameter = CategoriesInfo.AvailableParams.First();
            StringValue = string.Empty;
        }
    }

    public ObservableCollection<OperatorViewModel> AvailableOperators { get; } = [];

    public bool IsValueEditable {
        get => _isValueEditable;
        private set => RaiseAndSetIfChanged(ref _isValueEditable, value);
    }

    public string StringValue {
        get => _stringValue;
        set => RaiseAndSetIfChanged(ref _stringValue, value);
    }

    public OperatorViewModel? SelectedOperator {
        get => _selectedOperator;
        set => RaiseAndSetIfChanged(ref _selectedOperator, value);
    }

    public ParamViewModel? SelectedParameter {
        get => _selectedParameter;
        set => RaiseAndSetIfChanged(ref _selectedParameter, value);
    }

    public CategoriesInfoViewModel CategoriesInfo { get; }

    public Rule CreateRule() {
        if(SelectedParameter is null
           || SelectedOperator is null) {
            throw new InvalidOperationException();
        }

        return new Rule {
            OperatorKind = SelectedOperator.Operator,
            Param = SelectedParameter.ParamModel,
            Value = SelectedParameter.ParamModel.GetParamValueFromString(StringValue)
        };
    }

    public bool IsEmpty() {
        return SelectedParameter == null
               || SelectedOperator == null
               || (SelectedOperator.Operator is not OperatorKind.HasNoValue and not OperatorKind.HasValue
                   && string.IsNullOrWhiteSpace(StringValue));
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
    }

    private void RuleViewModelChangedHandler(object sender, PropertyChangedEventArgs e) {
        if(e.PropertyName == nameof(SelectedParameter)) {
            OnSelectedParamChanged();
        }

        if(e.PropertyName == nameof(SelectedOperator)) {
            OnSelectedOperatorChanged();
        }
    }

    private void OnSelectedParamChanged() {
        if(SelectedParameter != null) {
            AvailableOperators.Clear();
            var availableOperators = SelectedParameter.ParamModel.GetOperatorKinds().Select(o => _allOperators[o]);
            foreach(var op in availableOperators) {
                AvailableOperators.Add(op);
            }

            if(SelectedOperator == null
               || !AvailableOperators.Contains(SelectedOperator)) {
                SelectedOperator = AvailableOperators.First();
            }
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
}
