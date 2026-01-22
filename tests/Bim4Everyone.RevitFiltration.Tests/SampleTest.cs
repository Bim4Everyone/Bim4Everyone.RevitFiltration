using Autodesk.Revit.DB;

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;

using TUnit.Core.Executors;

namespace Bim4Everyone.RevitFiltration.Tests;

public class SampleTest : RevitApiTest {
    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Cities_Startup_IsNotEmpty() {
        var cities = Application.Cities.Cast<City>();

        await Assert.That(cities).IsNotEmpty();
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Create_XYZ_ValidDistance() {
        var point = Application.Create.NewXYZ(3, 4, 5);

        await Assert.That(point.DistanceTo(XYZ.Zero)).IsEqualTo(7).Within(0.1);
    }
}
