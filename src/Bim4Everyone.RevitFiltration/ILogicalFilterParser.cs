namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Сериализует и десериализует <see cref="ILogicalFilter" />.
/// </summary>
public interface ILogicalFilterParser {
    /// <summary>
    ///     Пытается десериализовать <see cref="ILogicalFilter" /> из заданной строки.
    /// </summary>
    /// <param name="content">Строка, полученная через метод <see cref="Serialize" />.</param>
    /// <param name="filter">Десериализованный фильтр.</param>
    /// <returns>True - если десериализация прошла успешно, иначе - False.</returns>
    bool TryParse(string content, out ILogicalFilter filter);

    /// <summary>
    ///     Сериализует фильтр в строку.
    /// </summary>
    /// <param name="filter">Заданный фильтр.</param>
    /// <returns>Сериализованный фильтр.</returns>
    string Serialize(ILogicalFilter filter);
}
