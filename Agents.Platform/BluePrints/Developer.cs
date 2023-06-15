using Agents.Platform.Properties;

namespace Agents.Platform.BluePrints
{
    public class Developer : BluePrint
    {
        public Developer() : base(nameof(Developer), "Produce working code based on requirements provided by ProductOwner")
        {
        }

        public override string? PromptTemplate => Resources.DeveloperTemplate;
    }
}
