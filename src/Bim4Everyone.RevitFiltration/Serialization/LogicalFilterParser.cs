using Bim4Everyone.RevitFiltration.Filtration;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Serialization;

internal class LogicalFilterParser : ILogicalFilterParser {
    private readonly JsonSerializerSettings _settings;

    public LogicalFilterParser() {
        _settings = new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.Objects,
            SerializationBinder = new JsonSerializationBinder(),
            Converters = [new ElementIdConverter()]
        };
    }

    public bool TryParse(string content, out ILogicalFilter? filter) {
        if(string.IsNullOrWhiteSpace(content)) {
            filter = null;
            return false;
        }

        try {
            filter = JsonConvert.DeserializeObject<Set>(content, _settings);
            return true;
        } catch(JsonSerializationException) {
            filter = null;
            return false;
        }
    }

    public string Serialize(ILogicalFilter filter) {
        if(filter == null) {
            throw new ArgumentNullException(nameof(filter));
        }

        return JsonConvert.SerializeObject(filter, _settings);
    }
}
