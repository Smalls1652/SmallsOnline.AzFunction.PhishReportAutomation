using Microsoft.Extensions.Logging;
using SmallsOnline.AzFunctions.PhishReportAutomation.Helpers;
using SmallsOnline.MsGraphClient.Models;

namespace SmallsOnline.AzFunctions.PhishReportAutomation.Services;

public class GraphClientService : IGraphClientService
{
    private readonly ILogger<GraphClientService> _logger;
    public GraphClientService(ILogger<GraphClientService> logger)
    {
        _logger = logger;

        _logger.LogInformation("Creating GraphClientService instance.");

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