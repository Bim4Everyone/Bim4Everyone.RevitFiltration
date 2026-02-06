using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Models.FilterModel;
using Bim4Everyone.RevitFiltration.Controls.Models.Visitor;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Utils;

internal class OperatorKindUtils {
    private static readonly Dictionary<OperatorKind, IVisitor> _visitorsDictionary = new() {
        { OperatorKind.Equals, new EqualsVisitor() },
        { OperatorKind.NotEquals, new NotEqualsVisitor() },
        { OperatorKind.HasValue, new HasValueVisitor() },
        { OperatorKind.HasNoValue, new HasNoValueVisitor() },
        { OperatorKind.Greater, new GreaterVisitor() },
        { OperatorKind.GreaterOrEqual, new GreaterOrEqualVisitor() },
        { OperatorKind.Less, new LessVisitor() },
        { OperatorKind.LessOrEqual, new LessOrEqualVisitor() },
        { OperatorKind.BeginsWith, new BeginsWithVisitor() },
        { OperatorKind.NotBeginsWith, new NotBeginsWithVisitor() },
        { OperatorKind.Contains, new ContainsVisitor() },
        { OperatorKind.NotContains, new NotContainsVisitor() },
        { OperatorKind.EndsWith, new EndsWithVisitor() },
        { OperatorKind.NotEndsWith, new NotEndsWithVisitor() }
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
