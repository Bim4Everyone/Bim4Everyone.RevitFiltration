using System.Windows.Threading;

namespace Bim4Everyone.RevitFiltration.Controls.Models.Utils;

internal class Delay {
    private readonly Action _action;
    private readonly DispatcherTimer _timer;

    public Delay(int interval, Action action) {
        _action = action;

        _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, interval) };
        _timer.Tick += OnTimerTick;
    }

    private void OnTimerTick(object sender, EventArgs e) {
        _timer.Stop();
        _action();
    }

    public void Action() {
        _timer.Stop();
        _timer.Start();
    }
}
