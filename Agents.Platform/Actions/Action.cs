using Proto;

namespace Agents.Platform.Actions;

public abstract class Action : IAction
{
    protected Action(string category, string name, params Parameter[] parameters)
    {
        Category = category;
        Name = name;
        Parameters = new List<Parameter>(parameters);
    }

    public string Category { get; }

    public string Name { get; }

    public List<Parameter> Parameters { get; }

    public abstract Task Execute(IContext context, IDictionary<string, string> parameters);

    public override string ToString()
    {
        return $"{Category}.{Name}";
    }
}