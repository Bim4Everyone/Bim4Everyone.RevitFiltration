using System.Globalization;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;

namespace Bim4Everyone.RevitFiltration.Controls.Services;

internal class LocalizationProvider : ILocalizationProvider {
    public LocalizationProvider() {
        InnerLocalization = new WpfLocalizationService(
            "pack://application:,,,/Bim4Everyone.RevitFiltration.Controls;component/assets/localization/language.xaml",
            CultureInfo.InstalledUICulture);
    }

    public ILocalizationService InnerLocalization { get; }

    public ILocalizationService? OutsourceLocalization { get; set; }

    public string GetLocalizedString(string name) {
        if(string.IsNullOrWhiteSpace(name)) {
            throw new ArgumentException(nameof(name));
        }

        if(OutsourceLocalization is not null) {
            return OutsourceLocalization.GetLocalizedString(name) ?? InnerLocalization.GetLocalizedString(name);
        }

        return InnerLocalization.GetLocalizedString(name);
    }

    public string GetLocalizedString(string name, params object[] args) {
        if(string.IsNullOrWhiteSpace(name)) {
            throw new ArgumentException(nameof(name));
        }

        if(OutsourceLocalization is not null) {
            return OutsourceLocalization.GetLocalizedString(name, args)
                   ?? InnerLocalization.GetLocalizedString(name, args);
        }

        return InnerLocalization.GetLocalizedString(name, args);
    }
}
