using Bim4Everyone.RevitFiltration.Controls.Core;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class OperatorViewModel : BaseViewModel {
    public OperatorViewModel(OperatorKind @operator) {
        Operator = @operator;
        Name = @operator.ToString(); // TODO
    }

    public OperatorKind Operator { get; }

    public string Name { get; }
}
