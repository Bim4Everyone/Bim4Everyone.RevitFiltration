using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

internal class Filter {
    public Set RootSet { get; set; } = new();

    public BuiltInCategory[] Categories { get; set; } = [];

    public ILogicalFilterContext GetLogicalFilterContext(ILogicalFilterFactory factory) {
        if(factory == null) {
            throw new ArgumentNullException(nameof(factory));
        }

        if(RootSet == null) {
            throw new InvalidOperationException();
        }

        return new LogicalFilterContext(this) { Factory = factory };
    }
}
