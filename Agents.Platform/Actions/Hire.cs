using Proto;

namespace Agents.Platform.Actions
{
    public class Hire : ActionBase
    {
        public Hire() : base("Recruitment", "Hire", new Parameter
        {
            Name = "Role",
            Description = "Role of agent to hire defined in its blueprint",
            Type = "string",
            IsRequired = true
        })
        {
        }
    }
}
