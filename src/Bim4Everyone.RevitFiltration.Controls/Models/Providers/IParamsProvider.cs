using Autodesk.Revit.DB;

using dosymep.Bim4Everyone;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Providers;

internal interface IParamsProvider {
    ICollection<RevitParam> GetParams(ICollection<Category> categories);
}
