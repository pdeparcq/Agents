using Proto;

namespace Agents.Platform.Actions
{
    public class Fire : Action
    {
        public Fire() : base("Recruitment", "Fire", new Parameter
        {
            Name = "Agent",
            Description = "Name of agent to fire",
            IsRequired = true,
            Type = "string"
        })
        {
        }

        public override Task Execute(IContext context, IDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
