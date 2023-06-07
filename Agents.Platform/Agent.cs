using Microsoft.Extensions.Logging;

namespace Agents.Platform
{
    public class Agent
    {
        private bool _isRunning;
        public BluePrint BluePrint { get; }
        public string Name { get; }

        public Agent(BluePrint bluePrint, string name)
        {
            BluePrint = bluePrint;
            Name = name;
        }

        public async Task Run(IEnvironment environment)
        {
            _isRunning = true;

            // Add some context to logging
            using (environment.Log.BeginScope(this))
            {
                // Loop while agent should keep running
                while (_isRunning)
                {
                    // Observe agent's environment
                    var observation = await Observe(environment);

                    if (observation.ShouldStop)
                    {
                        // Tell agent to stop running
                        environment.Log.LogInformation("Stopping...");
                        _isRunning = false;
                    }
                    else
                    {
                        // Act in the environment given the observation made
                        await Act(environment, observation);
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                }
            }
        }

        private async Task<Observation> Observe(IEnvironment environment)
        {
            environment.Log.LogInformation("Observing...");

            var actionsToTake = new List<ActionToTake>();

            if (BluePrint.Role == "Senior Developer")
            {
                var juniors = environment.Agents.Count(a => a.BluePrint.Role == "Junior Developer");

                if (juniors > 5)
                {
                    var junior = environment.Agents.First(a => a.BluePrint.Role == "Junior Developer");
                    
                    actionsToTake.Add(new ActionToTake
                    {
                        Name = "Fire",
                        ParameterValues = new Dictionary<string, string>{ {"Agent", junior.Name} }
                    });
                }
                else
                {
                    actionsToTake.Add(new ActionToTake
                    {
                        Name = "Hire",
                        ParameterValues = new Dictionary<string, string> { { "Role", "Junior Developer" } }
                    });
                }
            }

            return await Task.FromResult(new Observation
            {
                // Should stop if agent was removed from environment
                ShouldStop = environment.Agents.All(a => a.Name != Name),
                ActionsToTake = actionsToTake
            });
        }

        private async Task Act(IEnvironment environment, Observation observation)
        {
            environment.Log.LogInformation("Acting...");

            if (observation.ActionsToTake != null)
            {
                foreach (var actionToTake in observation.ActionsToTake)
                {
                    var action = environment.Actions.SingleOrDefault(a => a.Name == actionToTake.Name);

                    if (action != null)
                    {
                        try
                        {
                            environment.Log.LogInformation($"Executing action {action}...");
                            await action.Execute(environment, actionToTake.ParameterValues);
                        }
                        catch (Exception ex)
                        {
                            environment.Log.LogError(ex, $"Unable to execute action {action.Name}");
                        }
                    }
                    else
                    {
                        environment.Log.LogWarning($"No action found with name {actionToTake.Name}");
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
