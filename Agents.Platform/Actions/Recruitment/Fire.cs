using Proto;

namespace Agents.Platform.Actions.Recruitment
{
    public class Fire : Action
    {
        private readonly ITeam _team;

        public Fire(ITeam team) : base("Recruitment", "Fire", new Parameter
        {
            Name = "Agent",
            Description = "Name of agent to fire",
            IsRequired = true,
            Type = "string"
        })
        {
            _team = team;
        }

        public override async Task Execute(IContext context, IDictionary<string, string> parameters)
        {
            var agent = _team.Agents.SingleOrDefault(a => a.Name == parameters["Agent"]);

            if (agent == null)
            {
                throw new ArgumentException($"No agent with name {parameters["Agent"]} found");
            }

            // Stop the agent
            await context.StopAsync(_team.GetProcessId(agent));
        }
    }
}
