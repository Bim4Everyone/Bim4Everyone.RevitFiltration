using System.Windows.Input;

namespace Bim4Everyone.RevitFiltration.Controls.Core;

internal interface ICommand<T> : ICommand {
    void Execute(T parameter);
    bool CanExecute(T parameter);
}
