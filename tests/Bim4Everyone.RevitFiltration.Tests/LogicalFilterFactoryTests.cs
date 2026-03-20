using Bim4Everyone.RevitFiltration.Filtration;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public class LogicalFilterFactoryTests : RevitApiTest {
    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task CreateAndFilter_ReturnsNonNull() {
        var factory = new LogicalFilterFactory();

        var filter = factory.CreateAndFilter();

        await Assert.That(filter).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task CreateOrFilter_ReturnsNonNull() {
        var factory = new LogicalFilterFactory();

        var filter = factory.CreateOrFilter();

        await Assert.That(filter).IsNotNull();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task CreateAndFilter_ReturnsSeparateInstances() {
        var factory = new LogicalFilterFactory();

        var filter1 = factory.CreateAndFilter();
        var filter2 = factory.CreateAndFilter();

        await Assert.That(filter1).IsNotSameReferenceAs(filter2);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task CreateOrFilter_ReturnsSeparateInstances() {
        var factory = new LogicalFilterFactory();

        var filter1 = factory.CreateOrFilter();
        var filter2 = factory.CreateOrFilter();

        await Assert.That(filter1).IsNotSameReferenceAs(filter2);
    }
}
