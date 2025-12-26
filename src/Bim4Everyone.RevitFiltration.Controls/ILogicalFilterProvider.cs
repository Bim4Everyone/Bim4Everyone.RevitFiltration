namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Сервис для получения контекста фильтра по категориям, который пользователь задал в UI.
/// </summary>
public interface ILogicalFilterProvider {
    /// <summary>
    ///     Возвращает контекст фильтра, настроенного в UI. Перед вызовом надо проверить, что контекст может быть получен,
    ///     вызвав метод <see cref="CanGetFilter" />.
    /// </summary>
    /// <returns>Ссылка на контекст фильтра по категориям.</returns>
    /// <exception cref="System.InvalidOperationException">Исключение, если контекст не может быть получен.</exception>
    ILogicalFilterContext GetFilter();

    /// <summary>
    ///     Проверяет возможность получения контекста фильтра.
    /// </summary>
    /// <param name="errors">Ошибки, если контекст не может быть получен.</param>
    /// <returns>True, если контекст может быть получен, иначе False.</returns>
    bool CanGetFilter(out IErrorContext[] errors);

    /// <summary>
    ///     Возвращает провайдер данных.
    /// </summary>
    internal IDataProvider GetDataProvider();

    internal ILogicalFilterFactory GetLogicalFilterFactory();

    /// <summary>
    ///     Назначает ошибки, если ввод пользователя некорректный.
    /// </summary>
    internal void SetErrors(IErrorContext[] errors);

    /// <summary>
    ///     Назначает контекст фильтра, который пользователь сделал в UI.
    /// </summary>
    internal void SetFilter(ILogicalFilterContext filter);
}
