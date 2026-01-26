using Bim4Everyone.RevitFiltration.Controls.Models;
using Bim4Everyone.RevitFiltration.Controls.Serialization;

using Ninject;

namespace Bim4Everyone.RevitFiltration.Controls.Extensions;

/// <summary>
///     Расширения для настройки <see cref="IKernel" />.
/// </summary>
public static class NinjectExtensions {
    /// <summary>
    ///     Добавляет в контейнер <see cref="ILogicalFilterProviderFactory" />.
    ///     Также необходимо зарегистрировать в контейнере <see cref="ILogicalFilterFactory" />.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="System.ArgumentNullException">kernel is null.</exception>
    /// <exception cref="System.InvalidOperationException">В kernel не зарегистрирован ILogicalFilterFactory.</exception>
    public static IKernel UseLogicalFilterProviderFactory(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        if(kernel.TryGet<ILogicalFilterFactory>() is null) {
            throw new InvalidOperationException(
                $"Необходимо зарегистрировать в контейнере {nameof(ILogicalFilterFactory)}.");
        }

        kernel.Bind<ILogicalFilterProviderFactory>()
            .To<LogicalFilterProviderFactory>()
            .InSingletonScope();
        return kernel;
    }

    /// <summary>
    ///     Добавляет в контейнер <see cref="IFilterContextParser" />.
    ///     Также необходимо зарегистрировать в контейнере <see cref="ILogicalFilterFactory" />.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="System.ArgumentNullException">kernel is null.</exception>
    /// <exception cref="System.InvalidOperationException">В kernel не зарегистрирован ILogicalFilterFactory.</exception>
    public static IKernel UseFilterContextParser(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        if(kernel.TryGet<ILogicalFilterFactory>() is null) {
            throw new InvalidOperationException(
                $"Необходимо зарегистрировать в контейнере {nameof(ILogicalFilterFactory)}.");
        }

        kernel.Bind<IFilterContextParser>()
            .To<FilterContextParser>()
            .InSingletonScope();
        return kernel;
    }
}
