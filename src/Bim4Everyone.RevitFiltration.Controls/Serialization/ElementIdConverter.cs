using Autodesk.Revit.DB;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Controls.Serialization;

internal class ElementIdConverter : JsonConverter<ElementId> {
    public override void WriteJson(JsonWriter writer, ElementId? value, JsonSerializer serializer) {
#if REVIT2024_OR_GREATER
        writer.WriteValue(value?.Value);
#else
        writer.WriteValue(value?.IntegerValue);
#endif
    }

    public override ElementId ReadJson(
        JsonReader reader,
        Type objectType,
        ElementId? existingValue,
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
