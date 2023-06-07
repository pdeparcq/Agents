namespace Agents.Platform;

public interface IAgentAction
{
    string Category { get; }
    string Name { get; }
    List<Parameter> Parameters { get; }
    Task Execute(IEnvironment environment, IDictionary<string, string> parameterValues);
}