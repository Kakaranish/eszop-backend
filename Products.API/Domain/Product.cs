namespace Products.API.Domain
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
