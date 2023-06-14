namespace Agents.Platform
{
    public class ActionToTake
    {
        public string ActionName { get; set; }

        public IDictionary<string, string> ParameterValues { get; set; }
    }

    public class Observation
    {
        public List<ActionToTake>? ActionsToTake { get; set; }
    }
}
