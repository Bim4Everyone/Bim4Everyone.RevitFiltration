using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Providers;

internal interface IParamValuesProvider {
    ICollection<ParamValue> GetParamValues(ICollection<Category> categories, ParamModel param);
}
