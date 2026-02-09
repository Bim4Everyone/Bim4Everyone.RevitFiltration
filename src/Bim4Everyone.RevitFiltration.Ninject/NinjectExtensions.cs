using Bim4Everyone.RevitFiltration.Controls;
using Bim4Everyone.RevitFiltration.Controls.Models;
using Bim4Everyone.RevitFiltration.Controls.Serialization;
using Bim4Everyone.RevitFiltration.Filtration;
using Bim4Everyone.RevitFiltration.Serialization;

using Ninject;

namespace Bim4Everyone.RevitFiltration.Ninject;

/// <summary>
///     Расширения для настройки <see cref="IKernel" />.
/// </summary>
public static class NinjectExtensions {
    /// <summary>
    ///     Добавляет в контейнер <see cref="ILogicalFilterProviderFactory" />.
    ///     Также необходимо зарегистрировать в контейнере <see cref="ILogicalFilterFactory" /> через
    ///     <see cref="UseLogicalFilterFactory" />.
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
            .InTransientScope();
        return kernel;
    }

    /// <summary>
    ///     Добавляет в контейнер <see cref="IFilterContextParser" />.
    ///     Также необходимо зарегистрировать в контейнере <see cref="ILogicalFilterFactory" /> через
    ///     <see cref="UseLogicalFilterFactory" />.
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
            .InTransientScope();
        return kernel;
    }

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
            .InTransientScope();
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
            .InTransientScope();
        return kernel;
    }
}
