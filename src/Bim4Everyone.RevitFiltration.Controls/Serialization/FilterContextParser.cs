using Bim4Everyone.RevitFiltration.Controls.Models;
using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Controls.Serialization;

internal class FilterContextParser : IFilterContextParser {
    private readonly ILogicalFilterFactory _factory;
    private readonly JsonSerializerSettings _settings;

    public FilterContextParser(ILogicalFilterFactory factory) {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _settings = new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.Objects,
            SerializationBinder = new JsonSerializationBinder(),
            Converters = [new ElementIdConverter()]
        };
    }

    public bool TryParse(string content, out ILogicalFilterContext? context) {
        if(string.IsNullOrWhiteSpace(content)) {
            context = null;
            return false;
        }

        try {
            var ctx = JsonConvert.DeserializeObject<LogicalFilterContext>(content, _settings)
                      ?? new LogicalFilterContext(new Filter());
            ctx.Factory = _factory;
            context = ctx;
            return true;
        } catch(JsonSerializationException) {
            context = null;
            return false;
        }
    }

    public string Serialize(ILogicalFilterContext context) {
        if(context == null) {
            throw new ArgumentNullException(nameof(context));
        }

        return JsonConvert.SerializeObject(context, _settings);
    }
}
