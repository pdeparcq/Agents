using Agents.Platform.Messages;
using Microsoft.Extensions.Logging;
using Proto;
using Proto.Timers;

namespace Agents.Platform
{
    public class Agent : IActor
    {
        private readonly ILogger _logger;
        public BluePrint BluePrint { get; }
        public string Name { get; }

        public Agent(BluePrint bluePrint, string name, ILogger logger)
        {
            _logger = logger;
            BluePrint = bluePrint;
            Name = name;
        }

        public async Task ReceiveAsync(IContext context)
        {
            using (_logger.BeginScope(this))
            {
                if (context.Message is Started)
                {
                    context.Scheduler().RequestRepeatedly(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), context.Self,
                        new Tick());
                }
                if (context.Message is Tick)
                {
                    // Observe agent's environment
                    var observation = await Observe(context);

                    // Act in the environment given the observation made
                    await Act(context, observation);
                }
            }
        }

        private async Task<Observation> Observe(IContext context)
        {
            _logger.LogInformation($"Observing...");

            var actionsToTake = new List<Execute>();

            if (BluePrint.Role == "Senior Developer")
            {
                // TODO: hire or fire some juniors
            }

            return await Task.FromResult(new Observation
            {
                ActionsToTake = actionsToTake
            });
        }

        private async Task Act(IContext context, Observation observation)
        {
            _logger.LogInformation("Acting...");

            if (observation.ActionsToTake != null && context.Parent != null)
            {
                foreach (var actionToTake in observation.ActionsToTake)
                {
                    context.Send(context.Parent, actionToTake);
                }
            }
            
            await Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"{BluePrint.Role} {Name}";
        }
    }
}
