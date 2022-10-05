using Microsoft.Extensions.Logging;
using SmallsOnline.AzFunctions.PhishReportAutomation.Helpers;
using SmallsOnline.MsGraphClient.Models;

namespace SmallsOnline.AzFunctions.PhishReportAutomation.Services;

/// <summary>
/// Service that handles the GraphClient. Used for Dependency Injection.
/// </summary>
public class GraphClientService : IGraphClientService
{
    private readonly ILogger<GraphClientService> _logger;

    public GraphClientService(ILogger<GraphClientService> logger)
    {
        _logger = logger;

        _logger.LogInformation("Creating GraphClientService instance.");

        // Create a new GraphClient instance from the environment variables/app settings.
        GraphClient = new(
            baseUri: new("https://graph.microsoft.com/beta/"),
            clientId: AppSettings.GetSettingValue("AppId"),
            tenantId: AppSettings.GetSettingValue("TenantId"),
            credentialType: GraphClientCredentialType.Secret,
            clientSecret: AppSettings.GetSettingValue("ClientSecret"),
            apiScopes: new(
                new[]
                {
                    "https://graph.microsoft.com/.default"
                }
            )
        );

        _logger.LogInformation("GraphClientService instance created.");
    }

    public GraphClient GraphClient { get; }
}