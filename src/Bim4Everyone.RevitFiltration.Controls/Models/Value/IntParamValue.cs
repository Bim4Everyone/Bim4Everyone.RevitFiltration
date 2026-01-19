using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

using dosymep.Revit;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Value;

internal class IntParamValue : ParamValue<int> {
    public IntParamValue(int tValue, string displayValue)
        : base(tValue, displayValue) {
    }

    public override void AddInnerRule(ILogicalFilter logicalFilter, IVisitor visitor, ParamModel paramModel) {
        if(paramModel.Id.IsSystemId()) {
            visitor.AddInnerRule(logicalFilter, paramModel.Id.AsBuiltInParameter(), TValue);
        } else {
            visitor.AddInnerRule(logicalFilter, paramModel.Name, TValue);
        }
    }
}
