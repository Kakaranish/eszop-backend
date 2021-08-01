namespace Common.Utilities.Types
{
    public class EndpointConfig
    {
        public string Scheme { get; set; } = "http";
        public string Hostname { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return $"{Scheme}://{Hostname}:{Port}";
        }
    }
}
