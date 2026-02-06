namespace Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

internal class Set {
    public CompositorKind CompositorKind { get; set; } = CompositorKind.And;

    public List<Set> InnerSets { get; set; } = [];

    public List<Rule> InnerRules { get; set; } = [];

    public ILogicalFilter Generate(ILogicalFilterFactory factory) {
        var thisFilter = CompositorKind == CompositorKind.And
            ? factory.CreateAndFilter()
            : factory.CreateOrFilter();

        foreach(var innerRule in InnerRules) {
            innerRule.AddInnerRule(thisFilter);
        }

        foreach(var innerSet in InnerSets) {
            thisFilter.AddFilter(innerSet.Generate(factory));
        }

        return thisFilter;
    }
}
