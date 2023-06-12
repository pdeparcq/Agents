namespace Agents.Platform.Actions
{
    public class Fire : ActionBase
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
    }
}
