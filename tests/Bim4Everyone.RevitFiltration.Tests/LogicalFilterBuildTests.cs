using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Tests.Helpers;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public class LogicalFilterBuildTests : RevitApiTest {
    private readonly IOptions _options = new TestOptions();

    private Document OpenDocument() {
        string templatePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Autodesk",
            $"RVT {Application.VersionNumber}",
            "Templates",
            "English",
            "DefaultMetric.rte");
        return Application.NewProjectDocument(templatePath);
    }

    // --- Empty filter ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_NoRules_AndFilter_ReturnsElementIsElementTypeFilter() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter();

            var result = filter.Build(document, _options);

            await Assert.That(result).IsAssignableTo<ElementIsElementTypeFilter>();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_NoRules_OrFilter_ReturnsElementIsElementTypeFilter() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateOrFilter();

            var result = filter.Build(document, _options);

            await Assert.That(result).IsAssignableTo<ElementIsElementTypeFilter>();
        } finally {
            document.Close(false);
        }
    }

    // --- Null argument guards ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_NullDocument_ThrowsArgumentNullException() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "test");

        await Assert.That(() => filter.Build(null!, _options)).Throws<ArgumentNullException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_NullOptions_ThrowsArgumentNullException() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "test");

            await Assert.That(() => filter.Build(document, null!)).Throws<ArgumentNullException>();
        } finally {
            document.Close(false);
        }
    }

    // --- Compositor type ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_AndFilter_TwoRules_ReturnsLogicalAndFilter() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "a")
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "b");

            var result = filter.Build(document, _options);

            await Assert.That(result).IsAssignableTo<LogicalAndFilter>();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_OrFilter_TwoRules_ReturnsLogicalOrFilter() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateOrFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "a")
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "b");

            var result = filter.Build(document, _options);

            await Assert.That(result).IsAssignableTo<LogicalOrFilter>();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_WithNestedFilter_ReturnsLogicalAndFilter() {
        var document = OpenDocument();
        try {
            var factory = new LogicalFilterFactory();
            var inner = factory.CreateAndFilter();
            var outer = factory.CreateAndFilter().AddFilter(inner);

            var result = outer.Build(document, _options);

            await Assert.That(result).IsAssignableTo<LogicalAndFilter>();
        } finally {
            document.Close(false);
        }
    }

    // --- BuiltInParameter rule types ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_IntRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.PHASE_CREATED, 1);

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_DoubleRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_COST, 100.0);

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_StringRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "test");

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_ElementIdRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.ELEM_CATEGORY_PARAM, ElementId.InvalidElementId);

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_HasValueRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddHasValueRule(BuiltInParameter.ALL_MODEL_MARK);

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_HasNoValueRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddHasNoValueRule(BuiltInParameter.ALL_MODEL_MARK);

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_BeginsWithRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, "A");

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_ContainsRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, "A");

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_EndsWithRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddEndsWithRule(BuiltInParameter.ALL_MODEL_MARK, "A");

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_GreaterOrEqualRule_BuiltInParam_DoesNotThrow() {
        var document = OpenDocument();
        try {
            var filter = new LogicalFilterFactory().CreateAndFilter()
                .AddGreaterOrEqualRule(BuiltInParameter.ALL_MODEL_COST, 1.0);

            var result = filter.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    // --- Filtered-collector integration test helpers ---

    private Document OpenMepDocument() {
        string templatePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Autodesk",
            $"RVT {Application.VersionNumber}",
            "Templates",
            "English",
            "Systems-Default_Metric.rte");
        return Application.NewProjectDocument(templatePath);
    }

    private static Level GetDefaultLevel(Document doc) {
        return new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().First();
    }

    private static Wall CreateWall(Document doc, Level level, int index) {
        var line = Line.CreateBound(new XYZ(index * 10.0, 0, 0), new XYZ(index * 10.0 + 5.0, 0, 0));
        return Wall.Create(doc, line, level.Id, false);
    }

    private static Floor CreateFloor(Document doc, Level level, int index) {
        double offset = index * 20.0;
        var loop = new CurveLoop();
        loop.Append(Line.CreateBound(new XYZ(offset, 0, 0), new XYZ(offset + 10, 0, 0)));
        loop.Append(Line.CreateBound(new XYZ(offset + 10, 0, 0), new XYZ(offset + 10, 10, 0)));
        loop.Append(Line.CreateBound(new XYZ(offset + 10, 10, 0), new XYZ(offset, 10, 0)));
        loop.Append(Line.CreateBound(new XYZ(offset, 10, 0), new XYZ(offset, 0, 0)));
        var typeId = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).First().Id;
        return Floor.Create(doc, new List<CurveLoop> { loop }, typeId, level.Id);
    }

    private static Pipe CreatePipe(Document doc, Level level, int index) {
        var systemTypeId = new FilteredElementCollector(doc).OfClass(typeof(PipingSystemType)).First().Id;
        var pipeTypeId = new FilteredElementCollector(doc).OfClass(typeof(PipeType)).First().Id;
        return Pipe.Create(
            doc,
            systemTypeId,
            pipeTypeId,
            level.Id,
            new XYZ(index * 10.0, 0, 0),
            new XYZ(index * 10.0 + 5.0, 0, 0));
    }

    private static Duct CreateDuct(Document doc, Level level, int index) {
        var systemTypeId = new FilteredElementCollector(doc).OfClass(typeof(MechanicalSystemType)).First().Id;
        var ductTypeId = new FilteredElementCollector(doc).OfClass(typeof(DuctType)).First().Id;
        return Duct.Create(
            doc,
            systemTypeId,
            ductTypeId,
            level.Id,
            new XYZ(index * 10.0, 20, 0),
            new XYZ(index * 10.0 + 5.0, 20, 0));
    }

    private static Wall CreateWallWithLength(Document doc, Level level, int index, double lengthFeet) {
        double startX = index * 30.0;
        var line = Line.CreateBound(new XYZ(startX, 100.0, 0), new XYZ(startX + lengthFeet, 100.0, 0));
        return Wall.Create(doc, line, level.Id, false);
    }

    private static Floor CreateFloorWithSize(Document doc, Level level, int index, double sizeFeet) {
        double offsetX = index * 50.0;
        const double offsetY = 200.0;
        var loop = new CurveLoop();
        loop.Append(Line.CreateBound(new XYZ(offsetX, offsetY, 0), new XYZ(offsetX + sizeFeet, offsetY, 0)));
        loop.Append(
            Line.CreateBound(
                new XYZ(offsetX + sizeFeet, offsetY, 0),
                new XYZ(offsetX + sizeFeet, offsetY + sizeFeet, 0)));
        loop.Append(
            Line.CreateBound(
                new XYZ(offsetX + sizeFeet, offsetY + sizeFeet, 0),
                new XYZ(offsetX, offsetY + sizeFeet, 0)));
        loop.Append(Line.CreateBound(new XYZ(offsetX, offsetY + sizeFeet, 0), new XYZ(offsetX, offsetY, 0)));
        var typeId = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).First().Id;
        return Floor.Create(doc, new List<CurveLoop> { loop }, typeId, level.Id);
    }

    private static Pipe CreatePipeWithLength(Document doc, Level level, int index, double lengthFeet) {
        double startX = index * 30.0;
        var systemTypeId = new FilteredElementCollector(doc).OfClass(typeof(PipingSystemType)).First().Id;
        var pipeTypeId = new FilteredElementCollector(doc).OfClass(typeof(PipeType)).First().Id;
        return Pipe.Create(
            doc,
            systemTypeId,
            pipeTypeId,
            level.Id,
            new XYZ(startX, 100.0, 0),
            new XYZ(startX + lengthFeet, 100.0, 0));
    }

    private static Duct CreateDuctWithLength(Document doc, Level level, int index, double lengthFeet) {
        double startX = index * 30.0;
        var systemTypeId = new FilteredElementCollector(doc).OfClass(typeof(MechanicalSystemType)).First().Id;
        var ductTypeId = new FilteredElementCollector(doc).OfClass(typeof(DuctType)).First().Id;
        return Duct.Create(
            doc,
            systemTypeId,
            ductTypeId,
            level.Id,
            new XYZ(startX, 200.0, 0),
            new XYZ(startX + lengthFeet, 200.0, 0));
    }

    private static void RunInTransaction(Document doc, string name, Action action) {
        using var t = new Transaction(doc, name);
        t.Start();
        action();
        t.Commit();
    }

    private static int Collect(Document doc, BuiltInCategory cat, ElementFilter filter) {
        return new FilteredElementCollector(doc)
            .OfCategory(cat)
            .WhereElementIsNotElementType()
            .WherePasses(filter)
            .GetElementCount();
    }

    // --- Walls: filtered-collector integration tests ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_BeginsWithRule_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            string prefix = "WBW_" + Guid.NewGuid().ToString("N") + "_";
            string other = "WOTH_" + Guid.NewGuid().ToString("N") + "_";
            RunInTransaction(
                document,
                "Create walls",
                () => {
                    for(int i = 0; i < 3; i++) {
                        var wall = CreateWall(document, level, i);
                        wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(prefix + i);
                    }

                    for(int i = 3; i < 5; i++) {
                        var wall = CreateWall(document, level, i);
                        wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, prefix)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Walls, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_ContainsRule_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            string mid = "_WC_" + Guid.NewGuid().ToString("N") + "_";
            string other = "WOTH_" + Guid.NewGuid().ToString("N");
            RunInTransaction(
                document,
                "Create walls",
                () => {
                    for(int i = 0; i < 2; i++) {
                        var wall = CreateWall(document, level, i);
                        wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set("A" + mid + i);
                    }

                    for(int i = 2; i < 5; i++) {
                        var wall = CreateWall(document, level, i);
                        wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, mid)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Walls, elementFilter);
            await Assert.That(count).IsEqualTo(2);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_EndsWithRule_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            string suffix = "_WE_" + Guid.NewGuid().ToString("N");
            string other = "WOTH_" + Guid.NewGuid().ToString("N");
            RunInTransaction(
                document,
                "Create walls",
                () => {
                    for(int i = 0; i < 4; i++) {
                        var wall = CreateWall(document, level, i);
                        wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(i + suffix);
                    }

                    for(int i = 4; i < 6; i++) {
                        var wall = CreateWall(document, level, i);
                        wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddEndsWithRule(BuiltInParameter.ALL_MODEL_MARK, suffix)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Walls, elementFilter);
            await Assert.That(count).IsEqualTo(4);
        } finally {
            document.Close(false);
        }
    }

    // --- Floors: filtered-collector integration tests ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_BeginsWithRule_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            string prefix = "FBW_" + Guid.NewGuid().ToString("N") + "_";
            string other = "FOTH_" + Guid.NewGuid().ToString("N") + "_";
            RunInTransaction(
                document,
                "Create floors",
                () => {
                    for(int i = 0; i < 4; i++) {
                        var floor = CreateFloor(document, level, i);
                        floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(prefix + i);
                    }

                    for(int i = 4; i < 6; i++) {
                        var floor = CreateFloor(document, level, i);
                        floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, prefix)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Floors, elementFilter);
            await Assert.That(count).IsEqualTo(4);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_ContainsRule_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            string mid = "_FC_" + Guid.NewGuid().ToString("N") + "_";
            string other = "FOTH_" + Guid.NewGuid().ToString("N");
            RunInTransaction(
                document,
                "Create floors",
                () => {
                    for(int i = 0; i < 3; i++) {
                        var floor = CreateFloor(document, level, i);
                        floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set("X" + mid + i);
                    }

                    for(int i = 3; i < 5; i++) {
                        var floor = CreateFloor(document, level, i);
                        floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, mid)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Floors, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_EndsWithRule_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            string suffix = "_FE_" + Guid.NewGuid().ToString("N");
            string other = "FOTH_" + Guid.NewGuid().ToString("N");
            RunInTransaction(
                document,
                "Create floors",
                () => {
                    for(int i = 0; i < 2; i++) {
                        var floor = CreateFloor(document, level, i);
                        floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(i + suffix);
                    }

                    for(int i = 2; i < 4; i++) {
                        var floor = CreateFloor(document, level, i);
                        floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddEndsWithRule(BuiltInParameter.ALL_MODEL_MARK, suffix)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Floors, elementFilter);
            await Assert.That(count).IsEqualTo(2);
        } finally {
            document.Close(false);
        }
    }

    // --- Pipes: filtered-collector integration tests ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Pipes_BeginsWithRule_ReturnsExactCount() {
        var document = OpenMepDocument();
        try {
            var level = GetDefaultLevel(document);
            string prefix = "PBW_" + Guid.NewGuid().ToString("N") + "_";
            string other = "POTH_" + Guid.NewGuid().ToString("N") + "_";
            RunInTransaction(
                document,
                "Create pipes",
                () => {
                    for(int i = 0; i < 4; i++) {
                        var pipe = CreatePipe(document, level, i);
                        pipe.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(prefix + i);
                    }

                    for(int i = 4; i < 6; i++) {
                        var pipe = CreatePipe(document, level, i);
                        pipe.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, prefix)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_PipeCurves, elementFilter);
            await Assert.That(count).IsEqualTo(4);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Pipes_ContainsRule_ReturnsExactCount() {
        var document = OpenMepDocument();
        try {
            var level = GetDefaultLevel(document);
            string mid = "_PC_" + Guid.NewGuid().ToString("N") + "_";
            string other = "POTH_" + Guid.NewGuid().ToString("N");
            RunInTransaction(
                document,
                "Create pipes",
                () => {
                    for(int i = 0; i < 3; i++) {
                        var pipe = CreatePipe(document, level, i);
                        pipe.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set("X" + mid + i);
                    }

                    for(int i = 3; i < 5; i++) {
                        var pipe = CreatePipe(document, level, i);
                        pipe.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, mid)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_PipeCurves, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    // --- Ducts: filtered-collector integration tests ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Ducts_BeginsWithRule_ReturnsExactCount() {
        var document = OpenMepDocument();
        try {
            var level = GetDefaultLevel(document);
            string prefix = "DBW_" + Guid.NewGuid().ToString("N") + "_";
            string other = "DOTH_" + Guid.NewGuid().ToString("N") + "_";
            RunInTransaction(
                document,
                "Create ducts",
                () => {
                    for(int i = 0; i < 3; i++) {
                        var duct = CreateDuct(document, level, i);
                        duct.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(prefix + i);
                    }

                    for(int i = 3; i < 5; i++) {
                        var duct = CreateDuct(document, level, i);
                        duct.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, prefix)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_DuctCurves, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Ducts_ContainsRule_ReturnsExactCount() {
        var document = OpenMepDocument();
        try {
            var level = GetDefaultLevel(document);
            string mid = "_DC_" + Guid.NewGuid().ToString("N") + "_";
            string other = "DOTH_" + Guid.NewGuid().ToString("N");
            RunInTransaction(
                document,
                "Create ducts",
                () => {
                    for(int i = 0; i < 4; i++) {
                        var duct = CreateDuct(document, level, i);
                        duct.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set("X" + mid + i);
                    }

                    for(int i = 4; i < 6; i++) {
                        var duct = CreateDuct(document, level, i);
                        duct.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, mid)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_DuctCurves, elementFilter);
            await Assert.That(count).IsEqualTo(4);
        } finally {
            document.Close(false);
        }
    }

    // --- Walls: numeric length integration tests (CURVE_ELEM_LENGTH, internal feet) ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_LengthGreater_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create walls",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreateWallWithLength(document, level, i, 20.0);
                    }

                    for(int i = 3; i < 5; i++) {
                        CreateWallWithLength(document, level, i, 5.0);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddGreaterRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Walls, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_LengthLess_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create walls",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreateWallWithLength(document, level, i, 5.0);
                    }

                    for(int i = 3; i < 5; i++) {
                        CreateWallWithLength(document, level, i, 20.0);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddLessRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Walls, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_LengthGreaterOrEqual_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create walls",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreateWallWithLength(document, level, i, 10.0);
                    }

                    for(int i = 3; i < 5; i++) {
                        CreateWallWithLength(document, level, i, 5.0);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddGreaterOrEqualRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Walls, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    // --- Floors: numeric area integration tests (HOST_AREA_COMPUTED, internal sq feet) ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_AreaGreater_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create floors",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreateFloorWithSize(document, level, i, 15.0); // 225 sqft each
                    }

                    for(int i = 3; i < 5; i++) {
                        CreateFloorWithSize(document, level, i, 5.0); // 25 sqft each
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddGreaterRule(BuiltInParameter.HOST_AREA_COMPUTED, 100.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Floors, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_AreaLess_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create floors",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreateFloorWithSize(document, level, i, 5.0); // 25 sqft each
                    }

                    for(int i = 3; i < 5; i++) {
                        CreateFloorWithSize(document, level, i, 15.0); // 225 sqft each
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddLessRule(BuiltInParameter.HOST_AREA_COMPUTED, 100.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Floors, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_AreaLessOrEqual_ReturnsExactCount() {
        var document = OpenDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create floors",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreateFloorWithSize(document, level, i, 10.0); // 100 sqft each
                    }

                    for(int i = 3; i < 5; i++) {
                        CreateFloorWithSize(document, level, i, 20.0); // 400 sqft each
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddLessOrEqualRule(BuiltInParameter.HOST_AREA_COMPUTED, 100.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_Floors, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    // --- Pipes: numeric length integration tests (CURVE_ELEM_LENGTH, internal feet) ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Pipes_LengthGreater_ReturnsExactCount() {
        var document = OpenMepDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create pipes",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreatePipeWithLength(document, level, i, 20.0);
                    }

                    for(int i = 3; i < 5; i++) {
                        CreatePipeWithLength(document, level, i, 5.0);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddGreaterRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_PipeCurves, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Pipes_LengthLessOrEqual_ReturnsExactCount() {
        var document = OpenMepDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create pipes",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreatePipeWithLength(document, level, i, 10.0);
                    }

                    for(int i = 3; i < 5; i++) {
                        CreatePipeWithLength(document, level, i, 20.0);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddLessOrEqualRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_PipeCurves, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    // --- Ducts: numeric length integration tests (CURVE_ELEM_LENGTH, internal feet) ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Ducts_LengthGreaterOrEqual_ReturnsExactCount() {
        var document = OpenMepDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create ducts",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreateDuctWithLength(document, level, i, 10.0);
                    }

                    for(int i = 3; i < 5; i++) {
                        CreateDuctWithLength(document, level, i, 5.0);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddGreaterOrEqualRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_DuctCurves, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Ducts_LengthLess_ReturnsExactCount() {
        var document = OpenMepDocument();
        try {
            var level = GetDefaultLevel(document);
            RunInTransaction(
                document,
                "Create ducts",
                () => {
                    for(int i = 0; i < 3; i++) {
                        CreateDuctWithLength(document, level, i, 5.0);
                    }

                    for(int i = 3; i < 5; i++) {
                        CreateDuctWithLength(document, level, i, 20.0);
                    }
                });

            var elementFilter = new LogicalFilterFactory().CreateAndFilter()
                .AddLessRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
                .Build(document, _options);

            int count = Collect(document, BuiltInCategory.OST_DuctCurves, elementFilter);
            await Assert.That(count).IsEqualTo(3);
        } finally {
            document.Close(false);
        }
    }
}
