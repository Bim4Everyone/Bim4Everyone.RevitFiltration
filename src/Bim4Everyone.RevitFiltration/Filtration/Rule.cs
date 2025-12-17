using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.OperatorTokens;
using Bim4Everyone.RevitFiltration.Params;

namespace Bim4Everyone.RevitFiltration.Filtration;

internal class Rule : IFilterRule {
    public Rule(IParam param, IOperatorToken operatorToken) {
        Param = param;
        OperatorToken = operatorToken;
    }

    public IParam Param { get; }

    public IOperatorToken OperatorToken { get; }

    public FilterRule CreateFilterRule(Document document) {
        return OperatorToken.Create(Param.GetId(document));
    }
}
