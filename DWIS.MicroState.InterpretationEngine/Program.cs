using DWIS.Client.ReferenceImplementation.OPCFoundation;
using DWIS.MicroState.InterpretationEngine;


namespace DWIS.MicroState.InterpretationEngine
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
