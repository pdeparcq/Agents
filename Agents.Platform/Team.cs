using System.Collections.Concurrent;
using Agents.Platform.Actions;
using Microsoft.Extensions.Logging;

namespace Agents.Platform
{
    public class Team : IEnvironment
    {
        private readonly List<BluePrint> _bluePrints;
        private readonly List<IAgentAction> _actions;
        private readonly ConcurrentDictionary<Agent, Task?> _agents;
        private readonly INameGenerator _nameGenerator;

        public ILogger Log { get; }

        public IEnumerable<BluePrint> BluePrints => _bluePrints.AsEnumerable();

        public IEnumerable<IAgentAction> Actions => _actions.AsEnumerable();

        public IQueryable<Agent> Agents => _agents.Keys.AsQueryable();

        public Team(ILogger log, IEnumerable<BluePrint> bluePrints, INameGenerator nameGenerator)
        {
            Log = log;
            _bluePrints = new List<BluePrint>(bluePrints);
            _actions = new List<IAgentAction>()
            {
                new HireAction(),
                new FireAction()
            };
            _agents = new ConcurrentDictionary<Agent, Task?>();
            _nameGenerator = nameGenerator;
        }

        public void Hire(string role)
        {
            var bp = _bluePrints.SingleOrDefault(
                bp => bp.Role.Equals(role, StringComparison.InvariantCultureIgnoreCase));

            if (bp == null)
            {
                throw new ArgumentException($"No blueprint found for role {role}");
            }

            var agent = bp.Construct(_nameGenerator.GenerateName());
            _agents[agent] = null; // Make sure key is already available so that run method knows that agent is there
            _agents[agent] = agent.Run(this);
        }

        public void Fire(string agentName)
        {
            var agent = _agents.Keys.SingleOrDefault(a => a.Name == agentName);

            if (agent == null)
            {
                throw new ArgumentException($"No agent found with name {agentName}");
            }

            _agents.TryRemove(agent, out var t);
        }

        public async Task Work()
        {
            while(_agents.Values.Any(t => !t.IsCompleted))
            {

                await Task.WhenAll(_agents.Values);
            }
        }
    }
}
