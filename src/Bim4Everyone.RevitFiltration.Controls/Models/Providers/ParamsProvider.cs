using Autodesk.Revit.DB;

using dosymep.Bim4Everyone;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Providers;

internal class ParamsProvider : IParamsProvider {
    private readonly Func<ICollection<Category>, ICollection<RevitParam>> _getParams;

    public ParamsProvider(Func<ICollection<Category>, ICollection<RevitParam>> getParams) {
        _getParams = getParams ?? throw new ArgumentNullException(nameof(getParams));
    }

    public ICollection<RevitParam> GetParams(ICollection<Category> categories) {
        return _getParams(categories);
    }
}
