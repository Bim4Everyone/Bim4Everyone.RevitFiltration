using Bim4Everyone.RevitFiltration.Filtration;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Serialization;

/// <inheritdoc />
internal class LogicalFilterParser : ILogicalFilterParser {
    private readonly JsonSerializerSettings _settings;

    public LogicalFilterParser() {
        _settings = new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.Objects,
            SerializationBinder = new JsonSerializationBinder(),
            Converters = [new ElementIdConverter()]
        };
    }

    /// <inheritdoc />
    public bool TryParse(string content, out ILogicalFilter? filter) {
        if(string.IsNullOrWhiteSpace(content)) {
            filter = null;
            return false;
        }

        try {
            filter = JsonConvert.DeserializeObject<LogicalFilter>(content, _settings);
            return true;
        } catch(JsonException) {
            filter = null;
            return false;
        }
    }

    /// <inheritdoc />
    public string Serialize(ILogicalFilter filter) {
        if(filter == null) {
            throw new ArgumentNullException(nameof(filter));
        }

        return JsonConvert.SerializeObject(filter, _settings);
    }
}
