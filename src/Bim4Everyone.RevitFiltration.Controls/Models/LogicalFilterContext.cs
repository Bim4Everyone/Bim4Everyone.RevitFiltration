using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Controls.Models;

internal class LogicalFilterContext : ILogicalFilterContext {
    [JsonConstructor]
    public LogicalFilterContext(Filter filter) {
        Filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    [JsonIgnore]
    internal ILogicalFilterFactory Factory { get; set; }

    [JsonProperty]
    public Filter Filter { get; }

    [JsonIgnore]
    public ICollection<BuiltInCategory> SelectedCategories => Filter.Categories;

    public ILogicalFilter GetFilter() {
        if(Factory is null) {
            throw new InvalidOperationException();
        }

        return Filter.RootSet.Generate(Factory);
    }
}
