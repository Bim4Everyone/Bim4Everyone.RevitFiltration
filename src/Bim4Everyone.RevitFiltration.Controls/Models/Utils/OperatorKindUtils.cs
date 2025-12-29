using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Utils;

internal class OperatorKindUtils {
    private static readonly Dictionary<OperatorKind, IVisitor> _visitorsDictionary = new() {
        { OperatorKind.Equals, new EqualsVisitor() },
        { OperatorKind.NotEquals, new EqualsVisitor() },
        { OperatorKind.HasValue, new EqualsVisitor() },
        { OperatorKind.HasNoValue, new EqualsVisitor() },
        { OperatorKind.Greater, new EqualsVisitor() },
        { OperatorKind.GreaterOrEqual, new EqualsVisitor() },
        { OperatorKind.Less, new EqualsVisitor() },
        { OperatorKind.LessOrEqual, new EqualsVisitor() },
        { OperatorKind.BeginsWith, new EqualsVisitor() },
        { OperatorKind.NotBeginsWith, new EqualsVisitor() },
        { OperatorKind.Contains, new EqualsVisitor() },
        { OperatorKind.NotContains, new EqualsVisitor() },
        { OperatorKind.EndsWith, new EqualsVisitor() },
        { OperatorKind.NotEndsWith, new EqualsVisitor() }
    };

    public static ICollection<OperatorKind> GetOperatorKinds(StorageType storageType) {
        if(storageType == StorageType.String) {
            return [
                OperatorKind.Equals,
                OperatorKind.NotEquals,
                OperatorKind.Greater,
                OperatorKind.GreaterOrEqual,
                OperatorKind.Less,
                OperatorKind.LessOrEqual,
                OperatorKind.Contains,
                OperatorKind.NotContains,
                OperatorKind.BeginsWith,
                OperatorKind.NotBeginsWith,
                OperatorKind.EndsWith,
                OperatorKind.NotEndsWith,
                OperatorKind.HasValue,
                OperatorKind.HasNoValue
            ];
        }

        if(storageType == StorageType.ElementId) {
            return [OperatorKind.Equals, OperatorKind.NotEquals, OperatorKind.HasValue, OperatorKind.HasNoValue];
        }

        return [
            OperatorKind.Equals,
            OperatorKind.NotEquals,
            OperatorKind.Greater,
            OperatorKind.GreaterOrEqual,
            OperatorKind.Less,
            OperatorKind.LessOrEqual,
            OperatorKind.HasValue,
            OperatorKind.HasNoValue
        ];
    }

    public static IVisitor GetVisitor(OperatorKind operatorKind) {
        return _visitorsDictionary[operatorKind];
    }
}
