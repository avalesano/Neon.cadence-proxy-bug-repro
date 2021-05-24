using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neon.Cadence;

namespace bug_repro_5_0_0_fail
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Program.Main - start");
            CreateHostBuilder(args).Build().Run();
            Console.WriteLine("Program.Main - end");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            //Microsoft DI
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ExampleWorker>();
                    services.Configure<CadenceSettings>(hostContext.Configuration.GetSection(nameof(CadenceSettings)));
                });
        }
    }
}