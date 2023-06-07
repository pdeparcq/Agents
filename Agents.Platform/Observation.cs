namespace Agents.Platform
{
    public class ActionToTake
    {
        public string Name { get; set; }
        public IDictionary<string, string> ParameterValues { get; set; }
    }

    public class Observation
    {
        public bool ShouldStop { get; set; }
        public List<ActionToTake>? ActionsToTake { get; set; }
    }
}
