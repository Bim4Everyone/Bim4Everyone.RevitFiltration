using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Serialization;
using Bim4Everyone.RevitFiltration.Tests.Helpers;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public class LogicalFilterParserTests : RevitApiTest {
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

    private static ILogicalFilterParser CreateParser() {
        return new LogicalFilterParser();
    }

    private static ILogicalFilter CreateAndFilter() {
        return new LogicalFilterFactory().CreateAndFilter();
    }

    private static ILogicalFilter CreateOrFilter() {
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
        var document = OpenDocument();
        try {
            var parser = CreateParser();
            var original = CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "x");

            parser.TryParse(parser.Serialize(original), out var restored);
            var result = restored!.Build(document, _options);

            await Assert.That(result).IsAssignableTo<LogicalAndFilter>();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_OrFilter_BuildsToLogicalOrFilter() {
        var document = OpenDocument();
        try {
            var parser = CreateParser();
            var original = CreateOrFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "x");

            parser.TryParse(parser.Serialize(original), out var restored);
            var result = restored!.Build(document, _options);

            await Assert.That(result).IsAssignableTo<LogicalOrFilter>();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_WithIntRule_BuildsSuccessfully() {
        var document = OpenDocument();
        try {
            var parser = CreateParser();
            var original = CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.PHASE_CREATED, 1);

            parser.TryParse(parser.Serialize(original), out var restored);
            var result = restored!.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_WithStringRule_BuildsSuccessfully() {
        var document = OpenDocument();
        try {
            var parser = CreateParser();
            var original = CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "val");

            parser.TryParse(parser.Serialize(original), out var restored);
            var result = restored!.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task RoundTrip_WithNestedFilter_BuildsSuccessfully() {
        var document = OpenDocument();
        try {
            var parser = CreateParser();
            var inner = CreateOrFilter().AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "a");
            var original = CreateAndFilter()
                .AddEqualsRule(BuiltInParameter.ALL_MODEL_MARK, "b")
                .AddFilter(inner);

            parser.TryParse(parser.Serialize(original), out var restored);
            var result = restored!.Build(document, _options);

            await Assert.That(result).IsNotNull();
        } finally {
            document.Close(false);
        }
    }
}
