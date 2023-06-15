using Agents.Platform.Properties;

namespace Agents.Platform.BluePrints
{
    public class ProductOwner : BluePrint
    {
        public ProductOwner() : base(nameof(ProductOwner), "Gather requirements from business and provide them to developers")
        {
        }

        public override string? PromptTemplate => Resources.ProductOwnerTemplate;
    }
}
