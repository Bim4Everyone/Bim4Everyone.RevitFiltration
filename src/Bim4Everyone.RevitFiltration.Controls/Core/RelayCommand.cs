namespace Bim4Everyone.RevitFiltration.Controls.Core;

internal class RelayCommand : RelayCommand<object> {
    public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        : base(execute, canExecute) {
    }

    public static RelayCommand Create(Action execute, Func<bool>? canExecute = null) {
        return new RelayCommand(p => execute(), canExecute == null ? null : p => canExecute());
    }

    public static RelayCommand<T> Create<T>(Action<T> execute, Func<T, bool>? canExecute = null) {
        return new RelayCommand<T>(execute, canExecute);
    }
}
