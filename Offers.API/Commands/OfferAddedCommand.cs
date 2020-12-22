using MediatR;

namespace Offers.API.Commands
{
    public class OfferAddedCommand : IRequest
    {
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }

        public OfferAddedCommand(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}