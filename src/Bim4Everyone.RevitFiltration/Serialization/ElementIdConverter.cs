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
#if REVIT2024_OR_GREATER
        writer.WriteValue(value.Value);
#else
        writer.WriteValue(value.IntegerValue);
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

#if REVIT2024_OR_GREATER
        return new ElementId(Convert.ToInt64(reader.Value));
#else
        return new ElementId(Convert.ToInt32(reader.Value));
#endif
    }
}
