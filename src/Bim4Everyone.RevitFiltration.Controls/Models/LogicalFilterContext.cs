using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

namespace Bim4Everyone.RevitFiltration.Controls.Models;

internal class LogicalFilterContext : ILogicalFilterContext {
    public LogicalFilterContext(
        ICollection<BuiltInCategory> selectedCategories,
        ILogicalFilterFactory factory,
        Set set) {
        if(selectedCategories == null) {
            throw new ArgumentNullException(nameof(selectedCategories));
        }

        if(selectedCategories.Count == 0) {
            throw new ArgumentOutOfRangeException(nameof(selectedCategories));
        }

        SelectedCategories = selectedCategories;
        Set = set ?? throw new ArgumentNullException(nameof(set));
        Filter = set.Generate(factory);
    }

    public ILogicalFilter Filter { get; }
    public ICollection<BuiltInCategory> SelectedCategories { get; }
    public Set Set { get; }
}
