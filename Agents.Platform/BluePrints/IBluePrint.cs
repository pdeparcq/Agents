namespace Agents.Platform.BluePrints;

public interface IBluePrint
{
    string Role { get; }
    string Goal { get; }
    string? PromptTemplate { get; }
    IEnumerable<string> Actions { get; }
}