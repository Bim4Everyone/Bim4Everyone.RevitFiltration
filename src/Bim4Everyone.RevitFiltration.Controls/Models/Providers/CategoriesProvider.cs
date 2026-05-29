using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Providers;

internal class CategoriesProvider : ICategoriesProvider {
    private readonly ICollection<Category> _categories;

    public CategoriesProvider(ICollection<Category> categories) {
        _categories = categories ?? throw new ArgumentNullException(nameof(categories));
    }

    public ICollection<Category> GetCategories() {
        return _categories;
    }
}
