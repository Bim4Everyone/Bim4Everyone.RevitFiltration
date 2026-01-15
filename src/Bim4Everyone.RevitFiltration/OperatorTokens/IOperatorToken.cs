using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Operators;

namespace Bim4Everyone.RevitFiltration.OperatorTokens;

/// <summary>
///     Отвечает за вызов правильной перегрузки метода <see cref="IOperator.Create" />.
/// </summary>
internal interface IOperatorToken {
    /// <summary>
    ///     Оператор, определяющий условие фильтрации (больше, меньше, равно, содержит, без значения и т.д.).
    /// </summary>
    internal IOperator Operator { get; }

    /// <summary>
    ///     Создаёт правило на основе параметра и настроек точности.
    /// </summary>
    /// <param name="paramId">Параметр для проверки.</param>
    /// <param name="options">Настройки точности проверки.</param>
    /// <returns>Возвращает объект FilterRule из Revit.</returns>
    internal FilterRule Create(ElementId paramId, IOptions options);
}
