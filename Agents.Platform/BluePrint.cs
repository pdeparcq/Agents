using Microsoft.Extensions.Logging;

namespace Agents.Platform
{
    public class BluePrint
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
