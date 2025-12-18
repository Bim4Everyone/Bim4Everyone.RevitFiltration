using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Filtration;

internal class Rule : IFilterRule {
    [JsonConstructor]
    public Rule(IParam param, IOperatorToken operatorToken) {
        Param = param;
        OperatorToken = operatorToken;
    }

    [JsonProperty]
    public IParam Param { get; }

    [JsonProperty]
    public IOperatorToken OperatorToken { get; }

    public FilterRule CreateFilterRule(Document document, IOptions options) {
        return OperatorToken.Create(Param.GetId(document), options);
    }
}
