using Common.Types;

namespace Products.API.Domain
{
    public class Product : EntityBase, IAggregateRoot
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
