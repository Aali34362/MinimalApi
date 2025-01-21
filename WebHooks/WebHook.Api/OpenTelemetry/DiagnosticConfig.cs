using System.Diagnostics;

namespace WebHooks.OpenTelemetry;

internal sealed class DiagnosticConfig
{
    internal static readonly ActivitySource source = new("webhooks");
}
