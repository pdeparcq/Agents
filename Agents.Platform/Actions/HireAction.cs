namespace Agents.Platform.Actions
{
    public class HireAction : ActionBase
    {
        public HireAction() : base("Recruitment", "Hire", new Parameter
        {
            Name = "Role",
            Description = "Role of agent to hire defined in its blueprint",
            Type = "string",
            IsRequired = true
        })
        {
        }

        public override async Task Execute(IEnvironment environment, IDictionary<string, string> parameterValues)
        {
            environment.Hire(parameterValues["Role"]);

            await Task.CompletedTask;
        }
    }
}
