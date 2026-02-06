using Autodesk.Revit.DB;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Serialization;

/// <summary>
///     JSON-конвертер для типа <see cref="Autodesk.Revit.DB.ElementId" />.
/// </summary>
internal class ElementIdConverter : JsonConverter<ElementId> {
    /// <summary>
    ///     Записывает объект <see cref="Autodesk.Revit.DB.ElementId" /> в JSON.
    /// </summary>
    /// <param name="writer">Объект для записи JSON.</param>
    /// <param name="value">Экземпляр <see cref="Autodesk.Revit.DB.ElementId" /> для сериализации.</param>
    /// <param name="serializer">Сериализатор JSON.</param>
    public override void WriteJson(JsonWriter writer, ElementId value, JsonSerializer serializer) {
#if REVIT_2023_OR_LESS
        writer.WriteValue(value.IntegerValue);
#else
        writer.WriteValue(value.Value);
#endif
    }

    /// <summary>
    ///     Считывает объект <see cref="Autodesk.Revit.DB.ElementId" /> в JSON.
    /// </summary>
    /// <param name="reader">Объект для чтения JSON.</param>
    /// <param name="objectType">Тип объекта, в который производится десериализация.</param>
    /// <param name="existingValue">Существующее значение объекта.</param>
    /// <param name="hasExistingValue">Признак наличия существующего значения объекта.</param>
    /// <param name="serializer">Сериализатор JSON.</param>
    /// <returns>Десериализованное значение <see cref="Autodesk.Revit.DB.ElementId" />.</returns>
    public override ElementId ReadJson(
        JsonReader reader,
        Type objectType,
        ElementId existingValue,
        bool hasExistingValue,
        JsonSerializer serializer) {
        if(reader.Value is null) {
            return ElementId.InvalidElementId;
        }

#if REVIT_2023_OR_LESS
        return new ElementId(Convert.ToInt32(reader.Value));
#else
        return new ElementId(Convert.ToInt64(reader.Value));
#endif
    }
}
