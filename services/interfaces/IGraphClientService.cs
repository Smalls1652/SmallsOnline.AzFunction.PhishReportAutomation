using SmallsOnline.MsGraphClient.Models;

namespace SmallsOnline.AzFunctions.PhishReportAutomation.Services;

public interface IGraphClientService
{
    GraphClient GraphClient { get; }
}