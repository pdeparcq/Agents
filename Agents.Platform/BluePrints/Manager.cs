﻿using Agents.Platform.Properties;

namespace Agents.Platform.BluePrints
{
    public class Manager : BluePrint
    {
        public Manager() : base(nameof(Manager), "Hire or fire staff and facilitate communication between them")
        {
        }

        public override string? PromptTemplate => Resources.ManagerTemplate;

        public override IEnumerable<string> Actions => new List<string> { "Hire", "Fire" };
    }
}
