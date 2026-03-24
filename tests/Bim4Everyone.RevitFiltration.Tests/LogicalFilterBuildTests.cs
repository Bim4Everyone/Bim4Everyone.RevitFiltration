using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Tests.Helpers;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public class LogicalFilterBuildTests : RevitApiTest {
    private readonly IOptions _options = new TestOptions();
    private static Document _document = null!;

    [Before(Class)]
    [HookExecutor<RevitThreadExecutor>]
    public static void OpenDocument() {
        string templatePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Autodesk",
            $"RVT {Application.VersionNumber}",
            "Templates",
            "English",
            "DefaultMetric.rte");
        _document = Application.NewProjectDocument(templatePath);
    }

    [After(Class)]
    [HookExecutor<RevitThreadExecutor>]
    public static void CloseDocument() {
        _document.Close(false);
    }

    // --- Empty filter ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_NoRules_AndFilter_ReturnsElementIsElementTypeFilter() {
        var filter = new LogicalFilterFactory().CreateAndFilter();

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsAssignableTo<ElementIsElementTypeFilter>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_NoRules_OrFilter_ReturnsElementIsElementTypeFilter() {
        var filter = new LogicalFilterFactory().CreateOrFilter();

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsAssignableTo<ElementIsElementTypeFilter>();
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
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "test");

        await Assert.That(() => filter.Build(_document, null!)).Throws<ArgumentNullException>();
    }

    // --- Compositor type ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_AndFilter_TwoRules_ReturnsLogicalAndFilter() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "a")
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "b");

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsAssignableTo<LogicalAndFilter>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_OrFilter_TwoRules_ReturnsLogicalOrFilter() {
        var filter = new LogicalFilterFactory().CreateOrFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "a")
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "b");

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsAssignableTo<LogicalOrFilter>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_WithNestedFilter_ReturnsLogicalAndFilter() {
        var factory = new LogicalFilterFactory();
        var inner = factory.CreateAndFilter();
        var outer = factory.CreateAndFilter().AddFilter(inner);

        var result = outer.Build(_document, _options);

        await Assert.That(result).IsAssignableTo<LogicalAndFilter>();
    }

    // --- BuiltInParameter rule types ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_IntRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.PHASE_CREATED, 1);

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_DoubleRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_COST, 100.0);

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_StringRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "test");

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_ElementIdRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ELEM_CATEGORY_PARAM, ElementId.InvalidElementId);

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_HasValueRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddHasValueRule(BuiltInParameter.ALL_MODEL_MARK);

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_HasNoValueRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddHasNoValueRule(BuiltInParameter.ALL_MODEL_MARK);

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_BeginsWithRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddBeginsWithRule(BuiltInParameter.ALL_MODEL_MARK, "A");

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_ContainsRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddContainsRule(BuiltInParameter.ALL_MODEL_MARK, "A");

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_EndsWithRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddEndsWithRule(BuiltInParameter.ALL_MODEL_MARK, "A");

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Build_GreaterOrEqualRule_BuiltInParam_DoesNotThrow() {
        var filter = new LogicalFilterFactory().CreateAndFilter()
            .AddGreaterOrEqualRule(BuiltInParameter.ALL_MODEL_COST, 1.0);

        var result = filter.Build(_document, _options);

        await Assert.That(result).IsNotNull();
    }
}
