using Agents.Platform;
using Agents.Platform.Actions;
using Agents.Platform.Actions.Recruitment;
using Agents.Platform.BluePrints;
using Agents.Platform.Services;
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
            var props = system.DI().PropsFor<Agent>(new Manager());

            // Spawn that agent
            system.Root.Spawn(props);
            
            // Make sure agents stay alive until key press
            System.Console.ReadLine();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Proto
            services.AddSingleton(serviceProvider => new ActorSystem().WithServiceProvider(serviceProvider));

            // Services
            services.AddSingleton<INameGenerator>(sp => new NameGenerator());

            // Logging
            services.AddLogging(builder =>
            {
                builder.AddSimpleConsole(opt =>
                {
                    opt.IncludeScopes = true;
                    opt.SingleLine = false;
                }).SetMinimumLevel(LogLevel.Debug);
            });

            // Blueprints
            services.AddTransient<IBluePrint, Manager>();
            services.AddTransient<IBluePrint, ProductOwner>();
            services.AddTransient<IBluePrint, Developer>();

            // Actions
            services.AddTransient<IAction, Hire>();
            services.AddTransient<IAction, Fire>();

            // Team
            services.AddSingleton<ITeam, Team>();

            // Agent
            services.AddTransient<Agent>();
        }
    }
}