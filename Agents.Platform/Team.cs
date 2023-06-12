using Agents.Platform.Actions;
using Agents.Platform.Events;
using Agents.Platform.Messages;
using Microsoft.Extensions.Logging;
using Proto;

namespace Agents.Platform
{
    public class Team : IActor
    {
        private readonly List<BluePrint> _bluePrints;
        private readonly List<IAgentAction> _actions;
        private readonly INameGenerator _nameGenerator;
        private readonly ILogger _logger;

        public Team(INameGenerator nameGenerator, ILogger logger)
        {
            _bluePrints = new List<BluePrint>
            {
                new BluePrint("Senior Developer", "Assist junior developers"),
                new BluePrint("Junior Developer", "Produce working code")
            };
            _actions = new List<IAgentAction>()
            {
                new Hire(),
                new Fire()
            };
            _nameGenerator = nameGenerator;
            _logger = logger;
        }

        public async Task ReceiveAsync(IContext context)
        {
            using (_logger.BeginScope("Team"))
            {
                if (context.Message is Started)
                {
                    // Hire some people to get the party started
                    Hire(context, "Senior Developer");
                }
                if (context.Message is Execute exec)
                {
                    var action = _actions.Single(a => a.Name == exec.ActionName);

                    if (action is Hire)
                    {
                        Hire(context, exec.ParameterValues["Role"]);
                    }
                    if (action is Fire)
                    {
                        Fire(context, exec.ParameterValues["Agent"]);
                    }
                }

                await Task.CompletedTask;
            }
        }

        public void Hire(IContext context, string role)
        {
            _logger.LogInformation($"Hiring {role}...");

            context.Spawn(Props.FromProducer(() =>
                _bluePrints.Single(bp => bp.Role == role).Construct(_nameGenerator.GenerateName(), _logger)));
        }

        public void Fire(IContext context, string agentName)
        {
            _logger.LogInformation($"Firing {agentName}...");

            context.System.EventStream.Publish(new Fired { AgentName = agentName });
        }
    }
}
