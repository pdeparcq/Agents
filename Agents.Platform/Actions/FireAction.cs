namespace Agents.Platform.Actions
{
    public class FireAction : ActionBase
    {
        public FireAction() : base("Recruitment", "Fire", new Parameter
        {
            Name = "Agent",
            Description = "Name of agent to fire",
            IsRequired = true,
            Type = "string"
        })
        {
        }

        public override async Task Execute(IEnvironment environment, IDictionary<string, string> parameterValues)
        {
            environment.Fire(parameterValues["Agent"]);

            await Task.CompletedTask;
        }
    }
}
