using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration;

/// <summary>
///     Создает правила фильтрации элементов в Autodesk Revit.
/// </summary>
public interface ILogicalFilter {
    /// <summary>
    ///     Создает фильтр по элементам в Autodesk Revit с заданными настройками.
    /// </summary>
    /// <param name="document">Документ, в котором будут фильтроваться элементы.</param>
    /// <param name="options">Настройки генерации фильтра.</param>
    /// <returns>Фильтр по элементам.</returns>
    ElementFilter Build(Document document, IOptions options);

    /// <summary>
    ///     Добавляет вложенный ILogicalFilter.
    /// </summary>
    /// <param name="innerFilter">Вложенный фильтр.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddFilter(ILogicalFilter innerFilter);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "больше или равно" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddGreaterOrEqualRule(string paramName, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterOrEqualRule(string, int)" />
    ILogicalFilter AddGreaterOrEqualRule(string paramName, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterOrEqualRule(string, int)" />
    ILogicalFilter AddGreaterOrEqualRule(string paramName, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterOrEqualRule(string, int)" />
    ILogicalFilter AddGreaterOrEqualRule(string paramName, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "больше или равно" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterOrEqualRule(BuiltInParameter, int)" />
    ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterOrEqualRule(BuiltInParameter, int)" />
    ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterOrEqualRule(BuiltInParameter, int)" />
    ILogicalFilter AddGreaterOrEqualRule(BuiltInParameter paramId, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "больше" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddGreaterRule(string paramName, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterRule(string, int)" />
    ILogicalFilter AddGreaterRule(string paramName, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterRule(string, int)" />
    ILogicalFilter AddGreaterRule(string paramName, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterRule(string, int)" />
    ILogicalFilter AddGreaterRule(string paramName, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "больше" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddGreaterRule(BuiltInParameter paramId, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterRule(BuiltInParameter, int)" />
    ILogicalFilter AddGreaterRule(BuiltInParameter paramId, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterRule(BuiltInParameter, int)" />
    ILogicalFilter AddGreaterRule(BuiltInParameter paramId, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddGreaterRule(BuiltInParameter, int)" />
    ILogicalFilter AddGreaterRule(BuiltInParameter paramId, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "меньше или равно" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddLessOrEqualRule(string paramName, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessOrEqualRule(string, int)" />
    ILogicalFilter AddLessOrEqualRule(string paramName, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessOrEqualRule(string, int)" />
    ILogicalFilter AddLessOrEqualRule(string paramName, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessOrEqualRule(string, int)" />
    ILogicalFilter AddLessOrEqualRule(string paramName, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "меньше или равно" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessOrEqualRule(BuiltInParameter, int)" />
    ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessOrEqualRule(BuiltInParameter, int)" />
    ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessOrEqualRule(BuiltInParameter, int)" />
    ILogicalFilter AddLessOrEqualRule(BuiltInParameter paramId, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "меньше" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddLessRule(string paramName, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessRule(string, int)" />
    ILogicalFilter AddLessRule(string paramName, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessRule(string, int)" />
    ILogicalFilter AddLessRule(string paramName, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessRule(string, int)" />
    ILogicalFilter AddLessRule(string paramName, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "меньше" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddLessRule(BuiltInParameter paramId, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessRule(BuiltInParameter, int)" />
    ILogicalFilter AddLessRule(BuiltInParameter paramId, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessRule(BuiltInParameter, int)" />
    ILogicalFilter AddLessRule(BuiltInParameter paramId, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddLessRule(BuiltInParameter, int)" />
    ILogicalFilter AddLessRule(BuiltInParameter paramId, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "равно" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddEqualsRule(string paramName, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddEqualsRule(string, int)" />
    ILogicalFilter AddEqualsRule(string paramName, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddEqualsRule(string, int)" />
    ILogicalFilter AddEqualsRule(string paramName, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddEqualsRule(string, int)" />
    ILogicalFilter AddEqualsRule(string paramName, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "равно" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddEqualsRule(BuiltInParameter paramId, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddEqualsRule(BuiltInParameter, int)" />
    ILogicalFilter AddEqualsRule(BuiltInParameter paramId, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddEqualsRule(BuiltInParameter, int)" />
    ILogicalFilter AddEqualsRule(BuiltInParameter paramId, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddEqualsRule(BuiltInParameter, int)" />
    ILogicalFilter AddEqualsRule(BuiltInParameter paramId, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не равно" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddNotEqualsRule(string paramName, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddNotEqualsRule(string, int)" />
    ILogicalFilter AddNotEqualsRule(string paramName, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddNotEqualsRule(string, int)" />
    ILogicalFilter AddNotEqualsRule(string paramName, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddNotEqualsRule(string, int)" />
    ILogicalFilter AddNotEqualsRule(string paramName, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не равно" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, int paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddNotEqualsRule(BuiltInParameter, int)" />
    ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, double paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddNotEqualsRule(BuiltInParameter, int)" />
    ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, string paramValue);

    /// <inheritdoc cref="ILogicalFilter.AddNotEqualsRule(BuiltInParameter, int)" />
    ILogicalFilter AddNotEqualsRule(BuiltInParameter paramId, ElementId paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не имеет значение" для полученного параметра.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddHasNoValueRule(string paramName);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не имеет значение" для полученного параметра.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddHasNoValueRule(BuiltInParameter paramId);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "имеет значение" для полученного параметра.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddHasValueRule(string paramName);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "имеет значение" для полученного параметра.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddHasValueRule(BuiltInParameter paramId);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "начинается с" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddBeginsWithRule(string paramName, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "начинается с" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddBeginsWithRule(BuiltInParameter paramId, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не начинается с" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddNotBeginsWithRule(string paramName, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не начинается с" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddNotBeginsWithRule(BuiltInParameter paramId, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "заканчивается на" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddEndsWithRule(string paramName, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "заканчивается на" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddEndsWithRule(BuiltInParameter paramId, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не заканчивается на" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddNotEndsWithRule(string paramName, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не заканчивается на" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddNotEndsWithRule(BuiltInParameter paramId, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "содержит" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddContainsRule(string paramName, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "содержит" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddContainsRule(BuiltInParameter paramId, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не содержит" для полученного параметра и значения.
    ///     Работает для пользовательских параметров (общих, проекта и глобальных).
    /// </summary>
    /// <param name="paramName">Имя параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddNotContainsRule(string paramName, string paramValue);

    /// <summary>
    ///     Добавляет правило фильтрации с условием "не содержит" для полученного параметра и значения.
    ///     Работает для встроенных параметров (BuiltInParameter).
    /// </summary>
    /// <param name="paramId">Перечисление BuiltInParameter параметра для фильтрации.</param>
    /// <param name="paramValue">Значение параметра для фильтрации.</param>
    /// <returns>Ссылка на текущий <see cref="ILogicalFilter" />.</returns>
    ILogicalFilter AddNotContainsRule(BuiltInParameter paramId, string paramValue);
}
