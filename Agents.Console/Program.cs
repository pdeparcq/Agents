using Agents.Platform;
using Microsoft.Extensions.Logging;

namespace Agents.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Create a logger factory
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(opt =>
                {
                    opt.IncludeScopes = true;
                    opt.SingleLine = true;
                }); // Configure the console logger
            });

            var team = new Team(loggerFactory.CreateLogger<Program>(), new List<BluePrint>
            {
                new BluePrint("Senior Developer", "Assist junior developers"),
                new BluePrint("Junior Developer", "Produce working code")
            }, new NameGenerator());

            team.Hire("Senior Developer");

            await team.Work();
        }
    }
}