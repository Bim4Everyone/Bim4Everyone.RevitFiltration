using Bim4Everyone.RevitFiltration.Filtration;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public sealed class LogicalFilterArgumentValidationTests : RevitApiTest {
    private ILogicalFilter CreateFilter() {
        return new LogicalFilterFactory().CreateAndFilter();
    }

    // --- AddEqualsRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddEqualsRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddEqualsRule(null!, 1)).Throws<ArgumentException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddEqualsRule_EmptyParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddEqualsRule(string.Empty, 1)).Throws<ArgumentException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddEqualsRule_WhitespaceParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddEqualsRule("   ", 1)).Throws<ArgumentException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddEqualsRule_ReturnsThis() {
        var filter = CreateFilter();
        var result = filter.AddEqualsRule("param", 1);
        await Assert.That(result).IsSameReferenceAs(filter);
    }

    // --- AddNotEqualsRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddNotEqualsRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddNotEqualsRule(null!, 1)).Throws<ArgumentException>();
    }

    // --- AddGreaterRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddGreaterRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddGreaterRule(null!, 1)).Throws<ArgumentException>();
    }

    // --- AddGreaterOrEqualRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddGreaterOrEqualRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddGreaterOrEqualRule(null!, 1)).Throws<ArgumentException>();
    }

    // --- AddLessRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddLessRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddLessRule(null!, 1)).Throws<ArgumentException>();
    }

    // --- AddLessOrEqualRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddLessOrEqualRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddLessOrEqualRule(null!, 1)).Throws<ArgumentException>();
    }

    // --- AddHasValueRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddHasValueRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddHasValueRule(null!)).Throws<ArgumentException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddHasValueRule_EmptyParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddHasValueRule(string.Empty)).Throws<ArgumentException>();
    }

    // --- AddHasNoValueRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddHasNoValueRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddHasNoValueRule(null!)).Throws<ArgumentException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddHasNoValueRule_EmptyParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddHasNoValueRule(string.Empty)).Throws<ArgumentException>();
    }

    // --- AddBeginsWithRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddBeginsWithRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddBeginsWithRule(null!, "A")).Throws<ArgumentException>();
    }

    // --- AddNotBeginsWithRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddNotBeginsWithRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddNotBeginsWithRule(null!, "A")).Throws<ArgumentException>();
    }

    // --- AddContainsRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddContainsRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddContainsRule(null!, "A")).Throws<ArgumentException>();
    }

    // --- AddNotContainsRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddNotContainsRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddNotContainsRule(null!, "A")).Throws<ArgumentException>();
    }

    // --- AddEndsWithRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddEndsWithRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddEndsWithRule(null!, "A")).Throws<ArgumentException>();
    }

    // --- AddNotEndsWithRule ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddNotEndsWithRule_NullParamName_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddNotEndsWithRule(null!, "A")).Throws<ArgumentException>();
    }

    // --- AddFilter ---

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddFilter_NullInnerFilter_ThrowsArgumentNullException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddFilter(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddFilter_SelfReference_ThrowsArgumentException() {
        var filter = CreateFilter();
        await Assert.That(() => filter.AddFilter(filter)).Throws<ArgumentException>();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task AddFilter_CircularNested_ThrowsArgumentException() {
        var factory = new LogicalFilterFactory();
        var outer = factory.CreateAndFilter();
        var inner = factory.CreateAndFilter();
        outer.AddFilter(inner);
        await Assert.That(() => inner.AddFilter(outer)).Throws<ArgumentException>();
    }
}
