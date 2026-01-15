using System.Reflection;

using pyRevitLabs.Json;
using pyRevitLabs.Json.Serialization;

namespace Bim4Everyone.RevitFiltration.Serialization;

/// <summary>
///     Биндер для сопоставления типов и имён сборок при сериализации и десериализации JSON.
/// </summary>
internal class JsonSerializationBinder : ISerializationBinder {
    private readonly DefaultSerializationBinder _defaultBinder = new();

    /// <summary>
    ///     Определяет имя сборки и полное имя типа для указанного сериализуемого типа.
    /// </summary>
    /// <param name="serializedType">Сериализуемый тип.</param>
    /// <param name="assemblyName">Имя сборки сериализуемого типа.</param>
    /// <param name="typeName">Имя сериализуемого типа.</param>
    public void BindToName(Type serializedType, out string? assemblyName, out string? typeName) {
        if(serializedType.Assembly.GetName().Name.Equals(GetCurrentAssemblyName())) {
            assemblyName = GetCurrentAssemblyName();
            typeName = serializedType.FullName;
        } else {
            _defaultBinder.BindToName(serializedType, out assemblyName, out typeName);
        }
    }

    /// <summary>
    ///     Определяет тип по имени сборки и имени типа при десериализации JSON.
    /// </summary>
    /// <param name="assemblyName">Имя сборки из JSON.</param>
    /// <param name="typeName">Имя типа из JSON.</param>
    /// <returns>Тип, соответствующий указанным имени сборки и имени типа.</returns>
    /// <exception cref="JsonSerializationException">
    ///     в случае, если тип из текущей сборки не найден.
    /// </exception>
    public Type BindToType(string? assemblyName, string typeName) {
        return assemblyName?.Equals(GetCurrentAssemblyName()) ?? false
            ? Assembly.GetExecutingAssembly().GetType(typeName)
              ?? throw new JsonSerializationException($"Тип не найден: {typeName}")
            : _defaultBinder.BindToType(assemblyName, typeName);
    }

    /// <summary>
    ///     Возвращает имя текущей исполняемой сборки.
    /// </summary>
    private string GetCurrentAssemblyName() {
        return Assembly.GetExecutingAssembly().GetName().Name;
    }
}
