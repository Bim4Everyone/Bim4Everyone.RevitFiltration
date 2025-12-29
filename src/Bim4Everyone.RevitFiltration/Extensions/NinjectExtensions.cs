using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Serialization;

using Ninject;

namespace Bim4Everyone.RevitFiltration.Extensions;

/// <summary>
///     Расширения для настройки <see cref="IKernel" />.
/// </summary>
public static class NinjectExtensions {
    /// <summary>
    ///     Добавляет в контейнер <see cref="ILogicalFilterParser" />.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="System.ArgumentNullException">kernel is null.</exception>
    public static IKernel UseLogicalFilterParser(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<ILogicalFilterParser>()
            .To<LogicalFilterParser>()
            .InSingletonScope();
        return kernel;
    }

    /// <summary>
    ///     Добавляет в контейнер <see cref="ILogicalFilterFactory" />.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="System.ArgumentNullException">kernel is null.</exception>
    public static IKernel UseLogicalFilterFactory(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<ILogicalFilterFactory>()
            .To<LogicalFilterFactory>()
            .InSingletonScope();
        return kernel;
    }
}
