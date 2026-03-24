using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Tests.Helpers;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public class LogicalFilterBuildMepIntegrationTests : RevitApiTest {
    private readonly IOptions _options = new TestOptions();
    private Document _document = null!;

    [Before(Test)]
    [HookExecutor<RevitThreadExecutor>]
    public void OpenDocument() {
        string templatePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Autodesk",
            $"RVT {Application.VersionNumber}",
            "Templates",
            "English",
            "Systems-Default_Metric.rte");
        _document = Application.NewProjectDocument(templatePath);
    }

    [After(Test)]
    [HookExecutor<RevitThreadExecutor>]
    public void CloseDocument() {
        _document.Close(false);
    }

    private static Level GetDefaultLevel(Document doc) {
        return new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().First();
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

    // --- Pipes: string-rule integration tests ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Pipes_BeginsWithRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string prefix = "PBW_" + Guid.NewGuid().ToString("N") + "_";
        string other = "POTH_" + Guid.NewGuid().ToString("N") + "_";
        RunInTransaction(
            _document,
            "Create pipes",
            () => {
                for(int i = 0; i < 4; i++) {
                    var pipe = CreatePipe(_document, level, i);
                    pipe.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(prefix + i);
                }

                for(int i = 4; i < 6; i++) {
                    var pipe = CreatePipe(_document, level, i);
                    pipe.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, prefix)
            .Build(_document, _options);

        int count = Collect(_document, BuiltInCategory.OST_PipeCurves, elementFilter);
        await Assert.That(count).IsEqualTo(4);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Pipes_ContainsRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string mid = "_PC_" + Guid.NewGuid().ToString("N") + "_";
        string other = "POTH_" + Guid.NewGuid().ToString("N");
        RunInTransaction(
            _document,
            "Create pipes",
            () => {
                for(int i = 0; i < 3; i++) {
                    var pipe = CreatePipe(_document, level, i);
                    pipe.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set("X" + mid + i);
                }

                for(int i = 3; i < 5; i++) {
                    var pipe = CreatePipe(_document, level, i);
                    pipe.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, mid)
            .Build(_document, _options);

        int count = Collect(_document, BuiltInCategory.OST_PipeCurves, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    // --- Ducts: string-rule integration tests ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Ducts_BeginsWithRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string prefix = "DBW_" + Guid.NewGuid().ToString("N") + "_";
        string other = "DOTH_" + Guid.NewGuid().ToString("N") + "_";
        RunInTransaction(
            _document,
            "Create ducts",
            () => {
                for(int i = 0; i < 3; i++) {
                    var duct = CreateDuct(_document, level, i);
                    duct.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(prefix + i);
                }

                for(int i = 3; i < 5; i++) {
                    var duct = CreateDuct(_document, level, i);
                    duct.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, prefix)
            .Build(_document, _options);

        int count = Collect(_document, BuiltInCategory.OST_DuctCurves, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Ducts_ContainsRule_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        string mid = "_DC_" + Guid.NewGuid().ToString("N") + "_";
        string other = "DOTH_" + Guid.NewGuid().ToString("N");
        RunInTransaction(
            _document,
            "Create ducts",
            () => {
                for(int i = 0; i < 4; i++) {
                    var duct = CreateDuct(_document, level, i);
                    duct.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set("X" + mid + i);
                }

                for(int i = 4; i < 6; i++) {
                    var duct = CreateDuct(_document, level, i);
                    duct.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(other + i);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, mid)
            .Build(_document, _options);

        int count = Collect(_document, BuiltInCategory.OST_DuctCurves, elementFilter);
        await Assert.That(count).IsEqualTo(4);
    }

    // --- Pipes: numeric length integration tests (CURVE_ELEM_LENGTH, internal feet) ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Pipes_LengthGreater_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create pipes",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreatePipeWithLength(_document, level, i, 20.0);
                }

                for(int i = 3; i < 5; i++) {
                    CreatePipeWithLength(_document, level, i, 5.0);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddGreaterRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
            .Build(_document, _options);

        int count = Collect(_document, BuiltInCategory.OST_PipeCurves, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Pipes_LengthLessOrEqual_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create pipes",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreatePipeWithLength(_document, level, i, 10.0);
                }

                for(int i = 3; i < 5; i++) {
                    CreatePipeWithLength(_document, level, i, 20.0);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddLessOrEqualRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
            .Build(_document, _options);

        int count = Collect(_document, BuiltInCategory.OST_PipeCurves, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    // --- Ducts: numeric length integration tests (CURVE_ELEM_LENGTH, internal feet) ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Ducts_LengthGreaterOrEqual_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create ducts",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreateDuctWithLength(_document, level, i, 10.0);
                }

                for(int i = 3; i < 5; i++) {
                    CreateDuctWithLength(_document, level, i, 5.0);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddGreaterOrEqualRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
            .Build(_document, _options);

        int count = Collect(_document, BuiltInCategory.OST_DuctCurves, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task FilteredCollect_Ducts_LengthLess_ReturnsExactCount() {
        var level = GetDefaultLevel(_document);
        RunInTransaction(
            _document,
            "Create ducts",
            () => {
                for(int i = 0; i < 3; i++) {
                    CreateDuctWithLength(_document, level, i, 5.0);
                }

                for(int i = 3; i < 5; i++) {
                    CreateDuctWithLength(_document, level, i, 20.0);
                }
            });

        var elementFilter = new LogicalFilterFactory().CreateAndFilter()
            .AddLessRule(BuiltInParameter.CURVE_ELEM_LENGTH, 10.0)
            .Build(_document, _options);

        int count = Collect(_document, BuiltInCategory.OST_DuctCurves, elementFilter);
        await Assert.That(count).IsEqualTo(3);
    }
}
