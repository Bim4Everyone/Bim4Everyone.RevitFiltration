using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Filtration;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public sealed class LogicalFilterBuildArchIntegrationTests : RevitApiTest {
    private Document _document;
    private Options Options;

    [Before(Test)]
    [HookExecutor<RevitThreadExecutor>]
    public void OpenDocument() {
        string templatePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Autodesk",
            $"RVT {Application.VersionNumber}",
            "Templates",
            "English",
            "DefaultMetric.rte");
        _document = Application.NewProjectDocument(templatePath);
        Options = new Options();
    }

    [After(Test)]
    [HookExecutor<RevitThreadExecutor>]
    public void CloseDocument() {
        _document?.Close(false);
    }

    private Level GetDefaultLevel(Document doc) {
        return new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().First();
    }

    private Wall CreateWall(Document doc, Level level, int index) {
        var line = Line.CreateBound(new XYZ(index * 10.0, 0, 0), new XYZ(index * 10.0 + 5.0, 0, 0));
        return Wall.Create(doc, line, level.Id, false);
    }

    private Floor CreateFloor(Document doc, Level level, int index) {
        double offset = index * 20.0;
        var loop = new CurveLoop();
        loop.Append(Line.CreateBound(new XYZ(offset, 0, 0), new XYZ(offset + 10, 0, 0)));
        loop.Append(Line.CreateBound(new XYZ(offset + 10, 0, 0), new XYZ(offset + 10, 10, 0)));
        loop.Append(Line.CreateBound(new XYZ(offset + 10, 10, 0), new XYZ(offset, 10, 0)));
        loop.Append(Line.CreateBound(new XYZ(offset, 10, 0), new XYZ(offset, 0, 0)));
        var typeId = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).First().Id;
        return Floor.Create(doc, new List<CurveLoop> { loop }, typeId, level.Id);
    }

    private Wall CreateWallWithLength(Document doc, Level level, int index, double lengthFeet) {
        double startX = index * 30.0;
        var line = Line.CreateBound(new XYZ(startX, 100.0, 0), new XYZ(startX + lengthFeet, 100.0, 0));
        return Wall.Create(doc, line, level.Id, false);
    }

    private Floor CreateFloorWithSize(Document doc, Level level, int index, double sizeFeet) {
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

    private void RunInTransaction(Document doc, string name, Action action) {
        using var t = new Transaction(doc, name);
        t.Start();
        action();
        t.Commit();
    }

    private int Collect(Document doc, BuiltInCategory cat, ElementFilter filter) {
        return new FilteredElementCollector(doc)
            .OfCategory(cat)
            .WhereElementIsNotElementType()
            .WherePasses(filter)
            .GetElementCount();
    }

    // --- Walls: string-rule integration tests ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_BeginsWithRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string prefix = "WBW_" + Guid.NewGuid().ToString("N") + "_";
        string other = "WOTH_" + Guid.NewGuid().ToString("N") + "_";
        RunInTransaction(
            _document,
            "Create walls",
            () => {
                for(int i = 0; i < 3; i++) {
                    var wall = CreateWall(_document, level, i);
                    wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(prefix + i);
                }

                for(int i = 3; i < 5; i++) {
                    var wall = CreateWall(_document, level, i);
                    wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, prefix)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Walls, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_ContainsRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string mid = "_WC_" + Guid.NewGuid().ToString("N") + "_";
        string other = "WOTH_" + Guid.NewGuid().ToString("N");
        RunInTransaction(
            _document,
            "Create walls",
            () => {
                for(int i = 0; i < 2; i++) {
                    var wall = CreateWall(_document, level, i);
                    wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set("A" + mid + i);
                }

                for(int i = 2; i < 5; i++) {
                    var wall = CreateWall(_document, level, i);
                    wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, mid)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Walls, elementFilter);
        await Assert.That(count).IsEqualTo(2);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_EndsWithRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string suffix = "_WE_" + Guid.NewGuid().ToString("N");
        string other = "WOTH_" + Guid.NewGuid().ToString("N");
        RunInTransaction(
            _document,
            "Create walls",
            () => {
                for(int i = 0; i < 4; i++) {
                    var wall = CreateWall(_document, level, i);
                    wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(i + suffix);
                }

                for(int i = 4; i < 6; i++) {
                    var wall = CreateWall(_document, level, i);
                    wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddEndsWithRule(BuiltInParameter.ALL_MODEL_MARK, suffix)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Walls, elementFilter);
        await Assert.That(count).IsEqualTo(4);
    }

    // --- Floors: string-rule integration tests ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_BeginsWithRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string prefix = "FBW_" + Guid.NewGuid().ToString("N") + "_";
        string other = "FOTH_" + Guid.NewGuid().ToString("N") + "_";
        RunInTransaction(
            _document,
            "Create floors",
            () => {
                for(int i = 0; i < 4; i++) {
                    var floor = CreateFloor(_document, level, i);
                    floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(prefix + i);
                }

                for(int i = 4; i < 6; i++) {
                    var floor = CreateFloor(_document, level, i);
                    floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, prefix)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Floors, elementFilter);
        await Assert.That(count).IsEqualTo(4);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_ContainsRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string mid = "_FC_" + Guid.NewGuid().ToString("N") + "_";
        string other = "FOTH_" + Guid.NewGuid().ToString("N");
        RunInTransaction(
            _document,
            "Create floors",
            () => {
                for(int i = 0; i < 3; i++) {
                    var floor = CreateFloor(_document, level, i);
                    floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set("X" + mid + i);
                }

                for(int i = 3; i < 5; i++) {
                    var floor = CreateFloor(_document, level, i);
                    floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, mid)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Floors, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_EndsWithRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string suffix = "_FE_" + Guid.NewGuid().ToString("N");
        string other = "FOTH_" + Guid.NewGuid().ToString("N");
        RunInTransaction(
            _document,
            "Create floors",
            () => {
                for(int i = 0; i < 2; i++) {
                    var floor = CreateFloor(_document, level, i);
                    floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(i + suffix);
                }

                for(int i = 2; i < 4; i++) {
                    var floor = CreateFloor(_document, level, i);
                    floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddEndsWithRule(BuiltInParameter.ALL_MODEL_MARK, suffix)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Floors, elementFilter);
        await Assert.That(count).IsEqualTo(2);
    }

    // --- Walls: numeric length integration tests (CURVE_ELEM_LENGTH, internal feet) ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_LengthGreater_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create walls",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreateWallWithLength(_document, level, i, 20.0);
                }

                for(int i = 3; i < 5; i++) {
                    CreateWallWithLength(_document, level, i, 5.0);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddGreaterRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Walls, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_LengthLess_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create walls",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreateWallWithLength(_document, level, i, 5.0);
                }

                for(int i = 3; i < 5; i++) {
                    CreateWallWithLength(_document, level, i, 20.0);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddLessRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Walls, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Walls_LengthGreaterOrEqual_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create walls",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreateWallWithLength(_document, level, i, 10.0);
                }

                for(int i = 3; i < 5; i++) {
                    CreateWallWithLength(_document, level, i, 5.0);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddGreaterOrEqualRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Walls, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    // --- Floors: numeric area integration tests (HOST_AREA_COMPUTED, internal sq feet) ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_AreaGreater_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create floors",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreateFloorWithSize(_document, level, i, 15.0); // 225 sqft each
                }

                for(int i = 3; i < 5; i++) {
                    CreateFloorWithSize(_document, level, i, 5.0); // 25 sqft each
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddGreaterRule(BuiltInParameter.HOST_AREA_COMPUTED, 100.0)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Floors, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_AreaLess_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create floors",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreateFloorWithSize(_document, level, i, 5.0); // 25 sqft each
                }

                for(int i = 3; i < 5; i++) {
                    CreateFloorWithSize(_document, level, i, 15.0); // 225 sqft each
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddLessRule(BuiltInParameter.HOST_AREA_COMPUTED, 100.0)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Floors, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Floors_AreaLessOrEqual_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create floors",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreateFloorWithSize(_document, level, i, 10.0); // 100 sqft each
                }

                for(int i = 3; i < 5; i++) {
                    CreateFloorWithSize(_document, level, i, 20.0); // 400 sqft each
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddLessOrEqualRule(BuiltInParameter.HOST_AREA_COMPUTED, 100.0)
            .Build(_document, Options);

        int count = Collect(_document, BuiltInCategory.OST_Floors, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }
}
