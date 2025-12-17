namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Ошибка ввода пользователя в UI.
/// </summary>
public interface IErrorContext {
    /// <summary>
    ///     Сообщение об ошибке.
    /// </summary>
    string Message { get; }
}
