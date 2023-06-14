using Agents.Platform.BluePrints;
using Microsoft.Extensions.Logging;
using Proto;
using Proto.DependencyInjection;

namespace Agents.Platform.Actions
{
    public class Hire : Action
    {
        private readonly IEnumerable<IBluePrint> _bluePrints;
        
        public Hire(IEnumerable<IBluePrint> bluePrints) 
            : base("Recruitment", "Hire", new Parameter
        {
            Name = "Role",
            Description = "Role of agent to hire defined in its blueprint",
            Type = "string",
            IsRequired = true
        })
        {
            _bluePrints = bluePrints;
        }

        public override Task Execute(IContext context, IDictionary<string, string> parameters)
        {
            // TODO: check required parameters in base class

            var bluePrint = _bluePrints.SingleOrDefault(bp => bp.Role == parameters["Role"]);

            if (bluePrint != null)
            {
                var props = context.System.DI().PropsFor<Agent>(bluePrint);
                context.System.Root.Spawn(props);
            }
            else
            {
                throw new ArgumentException($"No blueprint found for role: {parameters["Role"]}");
            }

            return Task.CompletedTask;
        }
    }
}
