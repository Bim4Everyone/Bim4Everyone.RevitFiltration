using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Services;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class SetViewModel : BaseViewModel {
    private readonly ILocalizationProvider _localization;
    private readonly ILogicalFilterFactory _logicalFilterFactory;
    private CompositorViewModel _selectedCompositor;

    public SetViewModel(
        ILocalizationProvider localization,
        CategoriesInfoViewModel categoriesInfo,
        ILogicalFilterFactory logicalFilterFactory,
        Set? set = null) {
        CategoriesInfo =
            categoriesInfo ?? throw new ArgumentNullException(nameof(categoriesInfo));
        _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        _localization.LocalizationChanged += OnLocalizationChanged;
        _logicalFilterFactory = logicalFilterFactory ?? throw new ArgumentNullException(nameof(logicalFilterFactory));
        AvailableCompositors = new ReadOnlyCollection<CompositorViewModel>(
            [
                new CompositorViewModel(_localization, CompositorKind.And),
                new CompositorViewModel(_localization, CompositorKind.Or)
            ]
        );
        if(set != null) {
            SelectedCompositor = AvailableCompositors.First(c => c.CompositorKind == set.CompositorKind);
            InnerSets = [
                ..set.InnerSets.Select(s => new SetViewModel(_localization, CategoriesInfo, _logicalFilterFactory, s))
            ];
            InnerRules = [..set.InnerRules.Select(r => new RuleViewModel(_localization, CategoriesInfo, r))];
        } else {
            SelectedCompositor = AvailableCompositors.First();
        }

        foreach(var innerSet in InnerSets) {
            innerSet.PropertyChanged += OnInnerSetChanged;
        }

        foreach(var innerRule in InnerRules) {
            innerRule.PropertyChanged += OnInnerRuleChanged;
        }

        AddRuleCommand = RelayCommand.Create(AddRule, CanAddItems);
        AddSetCommand = RelayCommand.Create(AddSet, CanAddItems);
        RemoveRuleCommand = RelayCommand.Create<RuleViewModel>(RemoveRule, CanRemoveRule);
        RemoveSetCommand = RelayCommand.Create<SetViewModel>(RemoveSet, CanRemoveSet);
    }

    public IReadOnlyCollection<CompositorViewModel> AvailableCompositors { get; }

    public ObservableCollection<SetViewModel> InnerSets { get; } = [];

    public ObservableCollection<RuleViewModel> InnerRules { get; } = [];

    private CategoriesInfoViewModel CategoriesInfo { get; }

    public ICommand AddRuleCommand { get; }

    public ICommand AddSetCommand { get; }

    public ICommand RemoveSetCommand { get; }

    public ICommand RemoveRuleCommand { get; }

    public string AddRuleCommandName => _localization.GetLocalizedString("B4E.Filtration.Ui.AddRuleCommandName");

    public string AddSetCommandName => _localization.GetLocalizedString("B4E.Filtration.Ui.AddSetCommandName");

    public CompositorViewModel SelectedCompositor {
        get => _selectedCompositor;
        set => RaiseAndSetIfChanged(ref _selectedCompositor, value);
    }

    private void OnInnerRuleChanged(object sender, PropertyChangedEventArgs e) {
        NotifyInnerRulesChanges();
    }

    private void OnInnerSetChanged(object sender, PropertyChangedEventArgs e) {
        NotifyInnerSetsChanges();
    }

    /// <summary>
    ///     Сверяет вложенные наборы и правила с текущим списком доступных параметров,
    ///     сбрасывая те правила, параметры которых стали недоступны.
    /// </summary>
    public void ValidateParams() {
        foreach(var set in InnerSets) {
            set.ValidateParams();
        }

        foreach(var rule in InnerRules) {
            rule.ValidateParams();
        }
    }

    public Set CreateSet() {
        if(SelectedCompositor == null) {
            throw new InvalidOperationException();
        }

        return new Set {
            CompositorKind = SelectedCompositor.CompositorKind,
            InnerRules = InnerRules.Select(r => r.CreateRule()).ToList(),
            InnerSets = InnerSets.Select(s => s.CreateSet()).ToList()
        };
    }

    private void AddRule() {
        var vm = new RuleViewModel(_localization, CategoriesInfo);
        vm.PropertyChanged += OnInnerRuleChanged;
        InnerRules.Add(vm);
        NotifyInnerRulesChanges();
    }

    private void AddSet() {
        var vm = new SetViewModel(_localization, CategoriesInfo, _logicalFilterFactory);
        vm.PropertyChanged += OnInnerSetChanged;
        InnerSets.Add(vm);
        NotifyInnerSetsChanges();
    }

    private bool CanAddItems() {
        return CategoriesInfo.SelectedCategories.Count > 0;
    }

    private void RemoveSet(SetViewModel vm) {
        InnerSets.Remove(vm);
        vm.PropertyChanged -= OnInnerSetChanged;
        OnPropertyChanged(nameof(InnerSets));
    }

    private bool CanRemoveSet(SetViewModel? vm) {
        return vm != null;
    }

    private void RemoveRule(RuleViewModel vm) {
        InnerRules.Remove(vm);
        vm.PropertyChanged -= OnInnerRuleChanged;
        OnPropertyChanged(nameof(InnerRules));
    }

    private bool CanRemoveRule(RuleViewModel? vm) {
        return vm != null;
    }

    private void NotifyInnerSetsChanges() {
        OnPropertyChanged(nameof(InnerSets));
    }

    private void NotifyInnerRulesChanges() {
        OnPropertyChanged(nameof(InnerRules));
    }

    public bool IsEmpty() {
        return InnerRules.Any(r => r.IsEmpty())
               || CategoriesInfo.SelectedCategories.Count == 0;
    }

    public string GetErrorText() {
        return InnerRules.FirstOrDefault(item => !string.IsNullOrWhiteSpace(item.GetErrorText()))?.GetErrorText()
               ?? string.Empty;
    }

    private void OnLocalizationChanged(object sender, EventArgs e) {
        OnPropertyChanged(nameof(AddRuleCommandName));
        OnPropertyChanged(nameof(AddSetCommandName));
    }
}
