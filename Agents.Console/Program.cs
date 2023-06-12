using Agents.Platform;
using Microsoft.Extensions.Logging;
using Proto;
using Proto.Logging;

namespace Agents.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.SetLoggerFactory(LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(opt =>
                {
                    opt.IncludeScopes = true;
                    opt.SingleLine = true;
                }).SetMinimumLevel(LogLevel.Debug); // Configure the console logger
            }));
            
            var system = new ActorSystem();
            
            var props = Props.FromProducer(() => new Team(new NameGenerator()));

            system.Root.WithLoggingContext(Log.CreateLogger("Agents")).Spawn(props);

            System.Console.ReadLine();
        }
    }
}