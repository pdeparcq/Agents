﻿using System.Collections.Concurrent;
using Agents.Platform.Properties;
using HandlebarsDotNet;
using Proto;

namespace Agents.Platform
{
    public class Team : ITeam
    {
        private readonly ConcurrentDictionary<Agent, PID> _agents;

        public IEnumerable<Agent> Agents => _agents.Keys;

        public Team()
        {
            _agents = new ConcurrentDictionary<Agent, PID>();

            // Register partials
            Handlebars.RegisterTemplate("intro", Resources.IntroPartial);
            Handlebars.RegisterTemplate("outro", Resources.OutroPartial);
        }

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