using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Провайдер параметров для заданных категорий.
/// </summary>
public interface IParamsProvider {
    /// <summary>
    ///     Возвращает параметры, доступные сразу для всех заданных категорий.
    /// </summary>
    /// <param name="categories">Заданные категории.</param>
    /// <returns>Коллекция доступных параметров.</returns>
    ICollection<Parameter> GetParams(ICollection<Category> categories);
}
