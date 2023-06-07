namespace Agents.Platform
{
    public class NameGenerator : INameGenerator
    {
        private readonly HashSet<string> _usedNames;

        public NameGenerator()
        {
            _usedNames = new HashSet<string>();
        }

        public string GenerateName()
        {
            string[] firstNames = { "James", "Emma", "Oliver", "Sophia", "William", "Ava", "Benjamin", "Mia", "Henry", "Charlotte" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Taylor", "Davis", "Anderson", "Wilson", "Miller", "Moore", "Clark" };

            Random random = new Random();
            string name = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";

            while (_usedNames.Contains(name))
            {
                name = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
            }

            _usedNames.Add(name);
            return name;
        }
    }
}
