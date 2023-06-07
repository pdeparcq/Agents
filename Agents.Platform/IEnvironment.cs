using Microsoft.Extensions.Logging;

namespace Agents.Platform
{
    public interface IEnvironment
    {
        public ILogger Log { get; }

        public IEnumerable<BluePrint> BluePrints { get; }

        public IEnumerable<IAgentAction> Actions { get; }

        public IQueryable<Agent> Agents { get; }

        public void Hire(string role);

        public void Fire(string agentName);
    }
}
