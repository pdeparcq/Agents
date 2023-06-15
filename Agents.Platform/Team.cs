using System.Collections.Concurrent;
using Agents.Platform.BluePrints;
using Agents.Platform.Properties;
using HandlebarsDotNet;
using Proto;

namespace Agents.Platform
{
    public class Team : ITeam
    {
        private readonly ConcurrentDictionary<Agent, PID> _agents;

        public Team(IEnumerable<IBluePrint> bluePrints)
        {
            BluePrints = bluePrints;
            _agents = new ConcurrentDictionary<Agent, PID>();

            // Register partials
            Handlebars.RegisterTemplate("intro", Resources.IntroPartial);
            Handlebars.RegisterTemplate("outro", Resources.OutroPartial);
        }

        public IEnumerable<Agent> Agents => _agents.Keys;

        public IEnumerable<IBluePrint> BluePrints { get; }

        public void Register(Agent agent, PID pid)
        {
            _agents[agent] = pid;
        }

        public PID GetProcessId(Agent agent)
        {
            return _agents[agent];
        }

        public void UnRegister(Agent agent)
        {
            _agents.TryRemove(agent, out PID? value);
        }
    }
}
