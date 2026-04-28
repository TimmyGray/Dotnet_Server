using BuyingLibrary.AppSettings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Aspnet_server.Infrastructure;

public sealed class MailHealthCheck : IHealthCheck
{
    private readonly MailOptions _options;

    public MailHealthCheck(IOptions<MailOptions> options)
    {
        _options = options.Value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var configured = !string.IsNullOrWhiteSpace(_options.Email)
                         && !string.IsNullOrWhiteSpace(_options.Password)
                         && !string.IsNullOrWhiteSpace(_options.Host)
                         && _options.Port > 0;

        if (configured)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Mail settings are configured"));
        }

        return Task.FromResult(HealthCheckResult.Degraded("Mail settings are incomplete"));
    }
}
