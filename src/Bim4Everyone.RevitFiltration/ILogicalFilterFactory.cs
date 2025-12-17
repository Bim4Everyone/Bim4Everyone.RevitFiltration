namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Фабрика для создания <see cref="ILogicalFilter" />.
/// </summary>
public interface ILogicalFilterFactory {
    /// <summary>
    ///     Создает фильтр с условием "И".
    /// </summary>
    /// <returns>Ссылка на созданный фильтр.</returns>
    ILogicalFilter CreateAndFilter();

    /// <summary>
    ///     Создает фильтр с условием "Или".
    /// </summary>
    /// <returns>Ссылка на созданный фильтр.</returns>
    ILogicalFilter CreateOrFilter();
}
