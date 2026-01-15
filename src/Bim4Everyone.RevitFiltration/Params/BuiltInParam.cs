using Autodesk.Revit.DB;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Params;

/// <summary>
///     Класс для работы со встроенным BuiltInParam параметром Revit.
/// </summary>
internal class BuiltInParam : IParam {
    /// <summary>
    ///     Создание на основе перечисления BuiltInParameter.
    /// </summary>
    /// <param name="param">Значение перечисления BuiltInParameter искомого параметра.</param>
    [JsonConstructor]
    public BuiltInParam(BuiltInParameter param) {
        Param = param;
    }

    /// <summary>
    ///     Значение перечисления BuiltInParameter параметра.
    /// </summary>
    [JsonProperty]
    public BuiltInParameter Param { get; }

    /// <inheritdoc />
    public ElementId GetId(Document doc) {
        return new ElementId(Param);
    }
}
