using Agents.Platform.Actions;
using Agents.Platform.Messages;
using Proto;

namespace Agents.Platform
{
    public class Team : IActor
    {
        private readonly List<BluePrint> _bluePrints;
        private readonly List<IAgentAction> _actions;
        private readonly INameGenerator _nameGenerator;

        public Team(INameGenerator nameGenerator)
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
        }

        public async Task ReceiveAsync(IContext context)
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
            }

            await Task.CompletedTask;
        }

        public void Hire(IContext context, string role)
        {
            context.Spawn(Props.FromProducer(() =>
                _bluePrints.Single(bp => bp.Role == role).Construct(_nameGenerator.GenerateName())));
        }

        public void Fire(IContext context, string agentName)
        {
           // TODO: fire based on name
        }
    }
}
