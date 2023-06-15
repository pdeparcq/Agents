namespace Agents.Platform.BluePrints
{
    public abstract class BluePrint : IBluePrint
    {
        public string Role { get; }
        
        public string Goal { get; }

        public abstract string? PromptTemplate { get; }

        public abstract IEnumerable<string> Actions { get; }

        protected BluePrint(string role, string goal)
        {
            Role = role;
            Goal = goal;
        }
    }
}
