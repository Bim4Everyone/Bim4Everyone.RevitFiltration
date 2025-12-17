namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Фабрика для создания <see cref="ILogicalFilter" />.
/// </summary>
public interface ILogicalFilterFactory {
    /// <summary>
    ///     Создает фильтр с условием "И".
    /// </summary>
    /// <param name="options">Настройки генерации правил фильтрации параметров.</param>
    /// <returns>Ссылка на созданный фильтр.</returns>
    ILogicalFilter CreateAndFilter(IOptions options);

    /// <summary>
    ///     Создает фильтр с условием "Или".
    /// </summary>
    /// <param name="options">Настройки генерации правил фильтрации параметров.</param>
    /// <returns>Ссылка на созданный фильтр.</returns>
    ILogicalFilter CreateOrFilter(IOptions options);
}
