namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Аргументы события изменения контекста фильтра в <see cref="ILogicalFilterProvider" />.
/// </summary>
public sealed class FilterContextChangedEventArgs : EventArgs {
    /// <summary>
    ///     Создает аргументы события изменения контекста фильтра.
    /// </summary>
    /// <param name="oldContext">Предыдущий контекст фильтра (может быть null).</param>
    /// <param name="newContext">Новый контекст фильтра (может быть null, если контекст не может быть получен).</param>
    public FilterContextChangedEventArgs(
        ILogicalFilterContext? oldContext,
        ILogicalFilterContext? newContext) {
        OldContext = oldContext;
        NewContext = newContext;
    }

    /// <summary>
    ///     Предыдущий контекст фильтра. Null, если контекст ранее не был задан.
    /// </summary>
    public ILogicalFilterContext? OldContext { get; }

    /// <summary>
    ///     Новый контекст фильтра. Null, если контекст не может быть получен (например, при ошибках ввода).
    /// </summary>
    public ILogicalFilterContext? NewContext { get; }
}
