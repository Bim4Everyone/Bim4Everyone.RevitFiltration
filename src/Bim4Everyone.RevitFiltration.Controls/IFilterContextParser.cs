namespace Bim4Everyone.RevitFiltration.Controls;

/// <summary>
///     Сериализует и десериализует <see cref="ILogicalFilterContext" />.
/// </summary>
public interface IFilterContextParser {
    /// <summary>
    ///     Пытается десериализовать <see cref="ILogicalFilterContext" /> из заданной строки.
    /// </summary>
    /// <param name="content">Строка, полученная через метод <see cref="Serialize" />.</param>
    /// <param name="filter">Десериализованный контекст фильтра по категориям.</param>
    /// <returns>True - если десериализация прошла успешно, иначе - False.</returns>
    bool TryParse(string content, out ILogicalFilterContext context);

    /// <summary>
    ///     Сериализует контекст фильтра по категориям в строку.
    /// </summary>
    /// <param name="filter">Контекст фильтра по категориям.</param>
    /// <returns>Сериализованный контекст.</returns>
    string Serialize(ILogicalFilterContext context);
}
