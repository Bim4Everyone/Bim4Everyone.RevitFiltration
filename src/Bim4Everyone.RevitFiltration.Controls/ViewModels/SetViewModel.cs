using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using Bim4Everyone.RevitFiltration.Controls.Core;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class SetViewModel : BaseViewModel {
    private readonly ILogicalFilterFactory _logicalFilterFactory;
    private CompositorViewModel _selectedCompositor;

    public SetViewModel(
        CategoriesInfoViewModel categoriesInfo,
        ILogicalFilterFactory logicalFilterFactory,
        Set? set = null) {
        CategoriesInfo =
            categoriesInfo ?? throw new ArgumentNullException(nameof(categoriesInfo));
        _logicalFilterFactory = logicalFilterFactory ?? throw new ArgumentNullException(nameof(logicalFilterFactory));
        if(set != null) {
            SelectedCompositor = AvailableCompositors.First(c => c.CompositorKind == set.CompositorKind);
            InnerSets = [..set.InnerSets.Select(s => new SetViewModel(CategoriesInfo, _logicalFilterFactory, s))];
            InnerRules = [..set.InnerRules.Select(r => new RuleViewModel(CategoriesInfo, r))];
        } else {
            SelectedCompositor = AvailableCompositors.First();
        }

        foreach(var innerSet in InnerSets) {
            innerSet.PropertyChanged += OnInnerSetChanged;
        }

        foreach(var innerRule in InnerRules) {
            innerRule.PropertyChanged += OnInnerRuleChanged;
        }

        AddRuleCommand = RelayCommand.Create(AddRule);
        AddSetCommand = RelayCommand.Create(AddSet);
        RemoveRuleCommand = RelayCommand.Create<RuleViewModel>(RemoveRule, CanRemoveRule);
        RemoveSetCommand = RelayCommand.Create<SetViewModel>(RemoveSet, CanRemoveSet);
    }

    public IReadOnlyCollection<CompositorViewModel> AvailableCompositors { get; } =
        new ReadOnlyCollection<CompositorViewModel>(
            [new CompositorViewModel(CompositorKind.And), new CompositorViewModel(CompositorKind.Or)]
        );

    public ObservableCollection<SetViewModel> InnerSets { get; } = [];

    public ObservableCollection<RuleViewModel> InnerRules { get; } = [];

    private CategoriesInfoViewModel CategoriesInfo { get; }

    public ICommand AddRuleCommand { get; }

    public ICommand AddSetCommand { get; }

    public ICommand RemoveSetCommand { get; }

    public ICommand RemoveRuleCommand { get; }

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

    public void Renew() {
        foreach(var set in InnerSets) {
            set.Renew();
        }

        foreach(var rule in InnerRules) {
            rule.Renew();
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
        var vm = new RuleViewModel(CategoriesInfo);
        vm.PropertyChanged += OnInnerRuleChanged;
        InnerRules.Add(vm);
        NotifyInnerRulesChanges();
    }

    private void AddSet() {
        var vm = new SetViewModel(CategoriesInfo, _logicalFilterFactory);
        vm.PropertyChanged += OnInnerSetChanged;
        InnerSets.Add(vm);
        NotifyInnerSetsChanges();
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
}
