using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmallsOnline.AzFunctions.PhishReportAutomation.Services;

namespace SmallsOnline.AzFunctions.PhishReportAutomation;

public class Program
{
    public static void Main()
    {
        IHost host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(
                (services) =>
                {
                    services.AddSingleton<IGraphClientService, GraphClientService>();
                }
            )
            .Build();

        host.Run();
    }
}
