namespace Agents.Platform;

public abstract class ActionBase : IAgentAction
{
    protected ActionBase(string category, string name, params Parameter[] parameters)
    {
        Category = category;
        Name = name;
        Parameters = new List<Parameter>(parameters);
    }

    public string Category { get; }
    public string Name { get; }
    public List<Parameter> Parameters { get; }

    public abstract Task Execute(IEnvironment environment, IDictionary<string, string> parameterValues);

    public override string ToString()
    {
        return $"{Category}.{Name}";
    }
}