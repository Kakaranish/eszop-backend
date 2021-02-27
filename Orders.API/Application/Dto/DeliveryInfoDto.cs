namespace Orders.API.Application.Dto
{
    public class DeliveryInfoDto
    {
        public DeliveryAddressDto DeliveryAddress { get; init; }
        public DeliveryMethodDto DeliveryMethod { get; init; }
    }
}
