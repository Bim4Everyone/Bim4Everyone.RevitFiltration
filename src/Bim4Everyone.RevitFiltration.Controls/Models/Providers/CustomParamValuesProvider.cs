using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;

using dosymep.Bim4Everyone;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Providers;

internal class CustomParamValuesProvider : IParamValuesProvider {
    private readonly Func<ICollection<Category>, RevitParam, ICollection<string>> _getParamValues;

    public CustomParamValuesProvider(Func<ICollection<Category>, RevitParam, ICollection<string>> getParamValues) {
        _getParamValues = getParamValues ?? throw new ArgumentNullException(nameof(getParamValues));
    }

    public ICollection<ParamValue> GetParamValues(ICollection<Category> categories, ParamModel param) {
        if(param.RevitParam == null) {
            return [];
        }

        return _getParamValues(categories, param.RevitParam)
            .Select(param.GetParamValueFromString)
            .ToArray();
    }
}
