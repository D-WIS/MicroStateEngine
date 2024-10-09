using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace DWIS.MicroState.ThresholdsGenerator.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                             .ConfigureServices(services =>
                             {
                                 services.AddHostedService<Worker>();
                                 services.AddLogging(loggingBuilder => loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Debug));
                             })
                             .Build();

            host.Run();
        }
    }
}