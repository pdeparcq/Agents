using Proto;

namespace Agents.Platform;

public interface ITeam
{
    IEnumerable<Agent> Agents { get; }
    void Register(Agent agent, PID pid);
    void UnRegister(Agent agent);
    PID GetProcessId(Agent agent);
}