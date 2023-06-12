namespace Agents.Platform.Messages
{
    public class Execute
    {
        public string ActionName { get; set; }
        public IDictionary<string, string> ParameterValues { get; set; }
    }
}
