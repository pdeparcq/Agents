﻿using Agents.Platform.Actions;
using Agents.Platform.BluePrints;
using Agents.Platform.Messages;
using Agents.Platform.Services;
using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Proto;
using Proto.Timers;

namespace Agents.Platform
{
    public class Agent : IActor
    {
        private readonly ILogger<Agent> _logger;

        private readonly IEnumerable<IAction> _actions;

        private readonly HandlebarsTemplate<object, object> _promptTemplate;

        public Agent(IBluePrint bluePrint, INameGenerator nameGenerator, ILogger<Agent> logger, IEnumerable<IAction> actions, ITeam team)
        {
            _logger = logger;
            _actions = actions;
            Team = team;
            BluePrint = bluePrint;
            Name = nameGenerator.GenerateName();

            _promptTemplate = Handlebars.Compile(BluePrint.PromptTemplate);
        }

        public IBluePrint BluePrint { get; }

        public ITeam Team { get; }

        public string Name { get; }

        public string ExampleObservation
        {
            get
            {
                var observation = new Observation
                {
                    ActionsToTake = new List<ActionToTake>
                    {
                        new ActionToTake
                        {
                            ActionName = "Hire",
                            ParameterValues = new Dictionary<string, string>
                            {
                                { "Role", "Developer" }
                            }
                        }
                    },
                    Reason = "Need more developers!"
                };

                return JsonConvert.SerializeObject(observation, Formatting.Indented);
            }
        }

        public IEnumerable<IAction> Actions => _actions.Where(a => BluePrint.Actions.Contains(a.Name));

        public async Task ReceiveAsync(IContext context)
        {
            using (_logger.BeginScope(this))
            {
                if (context.Message is Started)
                {
                    // Register agent to team
                    Team.Register(this, context.Self);

                    // Schedule ticks
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
                if (context.Message is Stopped)
                {
                    // UnRegister agent from team
                    Team.UnRegister(this);
                }
            }
        }

        private async Task<Observation> Observe(IContext context)
        {
            _logger.LogInformation($"Observing...");

            var prompt = GeneratePrompt();

            _logger.LogInformation(prompt);
            _logger.LogWarning($"Words in prompt: {WordCount(prompt)}");

            return await Complete(prompt);
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

        private string GeneratePrompt()
        {
            return _promptTemplate(this);
        }

        private Task<Observation> Complete(string prompt)
        {
            return Task.FromResult(new Observation
            {
                Reason = "Nothing to do"
            });
        }

        private int WordCount(string prompt)
        {
            var delimiters = new char[] { ' ', '\r', '\n' };
            return prompt.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public override string ToString()
        {
            return $"{BluePrint.Role} {Name}";
        }
    }
}
