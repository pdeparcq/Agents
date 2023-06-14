using Agents.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proto;
using Proto.DependencyInjection;

namespace Agents.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configure dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            // Resolve actor system
            var system = provider.GetRequiredService<ActorSystem>();

            // Get props for first agent
            var props = system.DI().PropsFor<Agent>(new BluePrint("Manager", "Hire or fire agents"));

            // Spawn that agent
            system.Root.Spawn(props);
            
            // Make sure agents stay alive until key press
            System.Console.ReadLine();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(serviceProvider => new ActorSystem().WithServiceProvider(serviceProvider));
            services.AddSingleton<INameGenerator>(sp => new NameGenerator());
            services.AddLogging(builder =>
            {
                builder.AddSimpleConsole(opt =>
                {
                    opt.IncludeScopes = true;
                    opt.SingleLine = true;
                }).SetMinimumLevel(LogLevel.Debug);
            });
            services.AddTransient<Agent>();
        }
    }
}