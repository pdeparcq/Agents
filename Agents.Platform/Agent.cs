using Agents.Platform.Actions;
using Agents.Platform.BluePrints;
using Agents.Platform.Messages;
using Agents.Platform.Services;
using Microsoft.Extensions.Logging;
using Proto;
using Proto.Timers;

namespace Agents.Platform
{
    public class Agent : IActor
    {
        private readonly ILogger<Agent> _logger;

        private readonly IEnumerable<IAction> _actions;

        public IBluePrint BluePrint { get; }

        public string Name { get; }

        public Agent(IBluePrint bluePrint, INameGenerator nameGenerator, ILogger<Agent> logger, IEnumerable<IAction> actions)
        {
            _logger = logger;
            _actions = actions;
            BluePrint = bluePrint;
            Name = nameGenerator.GenerateName();
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

            var actionsToTake = new List<ActionToTake>();

            if (BluePrint.Role == "Manager")
            {
                actionsToTake.Add(new ActionToTake
                {
                    ActionName = "Hire",
                    ParameterValues = new Dictionary<string, string>()
                    {
                        {"Role", "Developer"}
                    }
                });
            }

            return await Task.FromResult(new Observation
            {
                ActionsToTake = actionsToTake
            });
        }

        private async Task Act(IContext context, Observation observation)
        {
            if (observation.ActionsToTake != null)
            {
                foreach (var actionToTake in observation.ActionsToTake)
                {
                    try
                    {
                        var action = _actions.SingleOrDefault(a => a.Name == actionToTake.ActionName);

                        if (action != null)
                        {
                            _logger.LogInformation($"Executing {action}");

                            await action.Execute(context, actionToTake.ParameterValues);
                        }
                        else
                        {
                            _logger.LogWarning($"No action found with name: {actionToTake.ActionName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to execute action {actionToTake.ActionName}");
                    }
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
