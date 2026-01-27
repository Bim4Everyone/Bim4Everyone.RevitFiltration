using System.Globalization;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;

namespace Bim4Everyone.RevitFiltration.Controls.Services;

internal class LocalizationProvider : ILocalizationProvider {
    private ILocalizationService? _outsourceLocalization;

    public LocalizationProvider() {
        InnerLocalization = new WpfLocalizationService(
            "pack://application:,,,/Bim4Everyone.RevitFiltration.Controls;component/assets/localization/language.xaml",
            CultureInfo.InstalledUICulture);
    }

    public ILocalizationService InnerLocalization { get; }

    public ILocalizationService? OutsourceLocalization {
        get => _outsourceLocalization;
        set {
            _outsourceLocalization = value;
            LocalizationChanged?.Invoke(this, EventArgs.Empty);
        }
    }

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

    public void SetInnerLocalization(CultureInfo cultureInfo) {
        InnerLocalization.SetLocalization(cultureInfo);
        LocalizationChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler LocalizationChanged;
}
