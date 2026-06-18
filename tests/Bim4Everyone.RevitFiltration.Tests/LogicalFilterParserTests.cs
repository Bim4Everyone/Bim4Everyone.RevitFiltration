using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Serialization;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public sealed class LogicalFilterParserTests : RevitApiTest {
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

    private ILogicalFilterParser CreateParser() {
        return new LogicalFilterParser();
    }

    private ILogicalFilter CreateAndFilter() {
        return new LogicalFilterFactory().CreateAndFilter();
    }

    private ILogicalFilter CreateOrFilter() {
        return new LogicalFilterFactory().CreateOrFilter();
    }

    // --- Serialize ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Serialize_NullFilter_ThrowsArgumentNullException() {
        var parser = CreateParser();
        await Assert.That(() => parser.Serialize(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Serialize_EmptyAndFilter_ReturnsNonEmptyString() {
        var parser = CreateParser();
        var filter = CreateAndFilter();

        string result = parser.Serialize(filter);

        await Assert.That(result).IsNotEmpty();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Serialize_EmptyOrFilter_ReturnsNonEmptyString() {
        var parser = CreateParser();
        var filter = CreateOrFilter();

        string result = parser.Serialize(filter);

        await Assert.That(result).IsNotEmpty();
    }

    // --- TryParse ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task TryParse_NullContent_ReturnsFalse() {
        var parser = CreateParser();

        bool success = parser.TryParse(null!, out _);

        await Assert.That(success).IsFalse();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task TryParse_EmptyContent_ReturnsFalse() {
        var parser = CreateParser();

        bool success = parser.TryParse(string.Empty, out _);

        await Assert.That(success).IsFalse();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task TryParse_WhitespaceContent_ReturnsFalse() {
        var parser = CreateParser();

        bool success = parser.TryParse("   ", out _);

        await Assert.That(success).IsFalse();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task TryParse_InvalidJson_ReturnsFalse() {
        var parser = CreateParser();

        bool success = parser.TryParse("not valid json {{{", out _);

        await Assert.That(success).IsFalse();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task TryParse_ValidJson_ReturnsTrueAndNonNullFilter() {
        var parser = CreateParser();
        string json = parser.Serialize(CreateAndFilter());

        bool success = parser.TryParse(json, out var filter);

        await Assert.That(success).IsTrue();
        await Assert.That(filter).IsNotNull();
    }

    // --- Round-trips ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_AndFilter_BuildsToLogicalAndFilter() {
        var parser = CreateParser();
        var original = CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "x");

        parser.TryParse(parser.Serialize(original), out var restored);
        var result = restored!.Build(_document, Options);

        await Assert.That(result).IsAssignableTo<LogicalAndFilter>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_OrFilter_BuildsToLogicalOrFilter() {
        var parser = CreateParser();
        var original = CreateOrFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "x");

        parser.TryParse(parser.Serialize(original), out var restored);
        var result = restored!.Build(_document, Options);

        await Assert.That(result).IsAssignableTo<LogicalOrFilter>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_WithIntRule_BuildsSuccessfully() {
        var parser = CreateParser();
        var original = CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.PHASE_CREATED, 1);

        parser.TryParse(parser.Serialize(original), out var restored);
        var result = restored!.Build(_document, Options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_WithStringRule_BuildsSuccessfully() {
        var parser = CreateParser();
        var original = CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "val");

        parser.TryParse(parser.Serialize(original), out var restored);
        var result = restored!.Build(_document, Options);

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_WithNestedFilter_BuildsSuccessfully() {
        var parser = CreateParser();
        var inner = CreateOrFilter().AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "a");
        var original = CreateAndFilter()
            .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "b")
            .AddFilter(inner);

        parser.TryParse(parser.Serialize(original), out var restored);
        var result = restored!.Build(_document, Options);

        await Assert.That(result).IsNotNull();
    }
}
