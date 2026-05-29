using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Providers;

internal interface ICategoriesProvider {
    ICollection<Category> GetCategories();
}
