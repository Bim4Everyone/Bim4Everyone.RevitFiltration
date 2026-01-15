using System.ComponentModel;

namespace Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;

internal enum OperatorKind {
    [Description("Равно")]
    Equals,

    [Description("Не равно")]
    NotEquals,

    [Description("Имеет значение")]
    HasValue,

    [Description("Без значения")]
    HasNoValue,

    [Description("Больше")]
    Greater,

    [Description("Больше или равно")]
    GreaterOrEqual,

    [Description("Меньше")]
    Less,

    [Description("Меньше или равно")]
    LessOrEqual,

    [Description("Начинается с")]
    BeginsWith,

    [Description("Не начинается с")]
    NotBeginsWith,

    [Description("Содержит")]
    Contains,

    [Description("Не содержит")]
    NotContains,

    [Description("Заканчивается на")]
    EndsWith,

    [Description("Не заканчивается на")]
    NotEndsWith
}
