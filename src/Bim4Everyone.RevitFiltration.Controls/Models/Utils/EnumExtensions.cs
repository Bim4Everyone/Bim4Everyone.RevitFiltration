using System.ComponentModel;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Utils;

internal static class EnumExtensions {
    public static string GetDescription<T>(this T e) where T : Enum {
        var type = e.GetType();
        string name = Enum.GetName(type, e)!;
        var info = type.GetField(name);

        var descriptionAttribute = info
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .OfType<DescriptionAttribute>()
            .FirstOrDefault();

        return descriptionAttribute?.Description ?? name;
    }
}
