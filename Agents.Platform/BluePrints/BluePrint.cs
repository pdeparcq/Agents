namespace Agents.Platform.BluePrints
{
    public class BluePrint : IBluePrint
    {
        public string Role { get; }
        public string Goal { get; }
        
        public BluePrint(string role, string goal)
        {
            Role = role;
            Goal = goal;
        }
    }
}
