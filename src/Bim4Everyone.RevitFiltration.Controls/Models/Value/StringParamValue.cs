using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Value;

internal class StringParamValue : ParamValue<string> {
    public StringParamValue(string tValue, string displayValue)
        : base(tValue, displayValue) {
    }

    public override void AddInnerRule(ILogicalFilter logicalFilter, IVisitor visitor, ParamModel paramModel) {
        if(paramModel.IsSystemParam(out var systemParam)) {
            visitor.AddInnerRule(logicalFilter, systemParam, TValue);
        } else {
            visitor.AddInnerRule(logicalFilter, paramModel.Name, TValue);
        }
    }
}
