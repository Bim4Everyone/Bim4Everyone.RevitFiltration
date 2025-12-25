using System.Windows.Input;

namespace Bim4Everyone.RevitFiltration.Controls.Core;

internal class RelayCommand<T> : ICommand<T> {
    private readonly Func<T, bool>? _canExecute;
    private readonly Action<T> _execute;

    public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null) {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    bool ICommand.CanExecute(object parameter) {
        return CanExecute((T) parameter);
    }

    void ICommand.Execute(object parameter) {
        Execute((T) parameter);
    }

    public bool CanExecute(T parameter) {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(T parameter) {
        try {
            _execute(parameter);
        } catch(OperationCanceledException) {
            // pass
        } catch(Autodesk.Revit.Exceptions.OperationCanceledException) {
            // pass
        }
    }
}
