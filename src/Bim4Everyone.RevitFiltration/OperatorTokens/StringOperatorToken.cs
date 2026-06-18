using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

using dosymep.Revit;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

/// <summary>
///     Настройки для проверки строкового параметра.
/// </summary>
internal class StringOperatorToken : IOperatorToken {
    /// <summary>
    ///     Создание токена для проверки строкового параметра.
    /// </summary>
    /// <param name="operator">
    ///     Оператор, определяющий условие фильтрации (больше, меньше, равно, содержит и т.д.)
    /// </param>
    /// <param name="value">Значение для проверки в фильтре.</param>
    [JsonConstructor]
    public StringOperatorToken(IOperator @operator, string value) {
        Operator = @operator;
        Value = value;
    }

    /// <summary>
    ///     Значение для проверки в фильтре.
    /// </summary>
    [JsonProperty]
    public string Value { get; }

    /// <inheritdoc />
    [JsonProperty]
    public IOperator Operator { get; }

    /// <inheritdoc />
    public FilterRule Create(ElementId paramId, Document document, Options options) {
        if(ParamStorageTypeIsElementId(paramId, document)
           && Operator is EqualsOperator or NotEqualsOperator) {
            // попытка получения элемента, являющегося значением параметра
            var idValue = new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .FirstOrDefault(item => item.Name.Equals(Value, StringComparison.CurrentCultureIgnoreCase))
                ?.Id;
            // попытка получения типоразмера, являющегося значением параметра
            idValue ??= new FilteredElementCollector(document)
                .WhereElementIsElementType()
                .WhereElementIsViewIndependent()
                .FirstOrDefault(item => item.Name.Equals(Value, StringComparison.CurrentCultureIgnoreCase))
                ?.Id;
            // попытка получения семейства и типоразмера, являющегося значением параметра
            // формат написания семейства и типоразмера в ревите:
            //     НазваниеСемейства : НазваниеТипа
            idValue ??= new FilteredElementCollector(document)
                .WhereElementIsElementType()
                .WhereElementIsViewIndependent()
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(item =>
                    (item.FamilyName + " : " + item.Name).Equals(Value, StringComparison.CurrentCultureIgnoreCase))
                ?.Id;
            if(idValue != null) {
                return Operator.Create(paramId, idValue);
            }
        }

        return Operator.Create(paramId, Value);
    }

    private bool ParamStorageTypeIsElementId(ElementId paramId, Document document) {
        if(paramId.IsSystemId()) {
            return document.get_TypeOfStorage(paramId.AsBuiltInParameter()) == StorageType.ElementId;
        }

        return ((ParameterElement) document.GetElement(paramId)).GetStorageType() == StorageType.ElementId;
    }
}
