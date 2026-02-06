using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Utils;
using Bim4Everyone.RevitFiltration.Controls.Models.Value;

namespace Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

internal class Rule {
    public OperatorKind OperatorKind { get; set; }
    public ParamValue Value { get; set; }
    public ParamModel Param { get; set; }

    public void AddInnerRule(ILogicalFilter filter) {
        Param.AddInnerRule(filter, OperatorKindUtils.GetVisitor(OperatorKind), Value);
    }
}
