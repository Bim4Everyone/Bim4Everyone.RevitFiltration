using Bim4Everyone.RevitFiltration.Controls.Models.Params;
using Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

using dosymep.Revit;

using pyRevitLabs.Json;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Value;

internal class ElementIdParamValue : ParamValue<string> {
    [JsonConstructor]
    public ElementIdParamValue(string value, string displayValue)
        : base(value, displayValue) {
    }

    public override void AddInnerRule(ILogicalFilter logicalFilter, IVisitor visitor, ParamModel paramModel) {
        // TODO проверить, что создание правила по строке, а не по ElementId работает
        if(paramModel.Id.IsSystemId()) {
            visitor.AddInnerRule(logicalFilter, paramModel.Id.AsBuiltInParameter(), TValue);
        } else {
            visitor.AddInnerRule(logicalFilter, paramModel.Name, TValue);
        }
    }
}
