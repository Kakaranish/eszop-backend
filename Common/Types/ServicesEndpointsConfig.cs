namespace Common.Types
{
    public class ServicesEndpointsConfig
    {
        public ServiceEndpoints Offers { get; set; }
        public ServiceEndpoints Carts { get; set; }
        public ServiceEndpoints Identity { get; set; }
        public ServiceEndpoints Orders { get; set; }
    }
}
