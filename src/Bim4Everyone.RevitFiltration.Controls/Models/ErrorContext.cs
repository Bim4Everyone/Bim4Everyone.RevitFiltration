namespace Bim4Everyone.RevitFiltration.Controls.Models;

internal class ErrorContext : IErrorContext {
    public ErrorContext(string message) {
        if(string.IsNullOrWhiteSpace(message)) {
            throw new ArgumentException(nameof(message));
        }

        Message = message;
    }

    public string Message { get; }
}
