using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;

using dosymep.Revit;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Providers;

internal class InstancesParamValuesProvider : IParamValuesProvider {
    private readonly ICollection<Document> _documents;

    public InstancesParamValuesProvider(ICollection<Document> documents) {
        _documents = documents ?? throw new ArgumentNullException(nameof(documents));
    }

    public ICollection<ParamValue> GetParamValues(ICollection<Category> categories, ParamModel param) {
        return _documents
            .SelectMany(d => GetParamValues(d, categories, param))
            .Distinct()
            .ToArray();
    }

    private ICollection<ParamValue> GetParamValues(
        Document doc,
        ICollection<Category> categories,
        ParamModel param) {
        return new FilteredElementCollector(doc)
            .WhereElementIsNotElementType()
            .WherePasses(new ElementMulticategoryFilter(categories.Select(c => c.GetBuiltInCategory()).ToArray()))
            .Where(e => e.IsExistsParamValue(param.Name))
            .Select(e => param.GetParamValueFromString(e.GetParam(param.Name).AsValueString()))
            .ToArray();
    }
}
