using Autodesk.Revit.DB;

namespace Bim4Everyone.RevitFiltration.Operators;

/// <summary>
///     Создает правило фильтрации элементов в Revit (больше, больше или равно, меньше, содержит, без значения и т.д.).
/// </summary>
internal interface IOperator {
    /// <summary>
    ///     Создаёт правило на основе параметра.
    /// </summary>
    /// <param name="paramId">Id параметра для проверки.</param>
    /// <returns>Возвращает объект FilterRule из Revit.</returns>
    internal FilterRule Create(ElementId paramId);

    /// <summary>
    ///     Создаёт правило на основе параметра и целочисленного значения.
    /// </summary>
    /// <param name="paramId">Id параметра для проверки.</param>
    /// <param name="value">Значение для проверки.</param>
    /// <returns>Возвращает объект FilterRule из Revit.</returns>
    internal FilterRule Create(ElementId paramId, int value);

    /// <summary>
    ///     Создаёт правило на основе параметра и значения числа с плавающей точкой.
    /// </summary>
    /// <param name="paramId">Id параметра для проверки.</param>
    /// <param name="value">Значение для проверки.</param>
    /// <param name="epsilon">Точность проверки.</param>
    /// <returns>Возвращает объект FilterRule из Revit.</returns>
    internal FilterRule Create(ElementId paramId, double value, double epsilon);

    /// <summary>
    ///     Создаёт правило на основе параметра и строкового значения.
    /// </summary>
    /// <param name="paramId">Id параметра для проверки.</param>
    /// <param name="value">Значение для проверки.</param>
    /// <returns>Возвращает объект FilterRule из Revit.</returns>
    internal FilterRule Create(ElementId paramId, string value);

    /// <summary>
    ///     Создаёт правило на основе параметра и значения типа ElementId.
    /// </summary>
    /// <param name="paramId">Id параметра для проверки.</param>
    /// <param name="value">Значение для проверки.</param>
    /// <returns>Возвращает объект FilterRule из Revit.</returns>
    internal FilterRule Create(ElementId paramId, ElementId value);
}
