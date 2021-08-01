using Newtonsoft.Json;

namespace Common.Utilities.ErrorHandling
{
    public class Error
    {
        [JsonProperty]
        public string Message { get; set; }
        
        public string Code { get; set; }

        public Error()
        {
        }

        public Error(string message)
        {
            Message = message;
        }
    }
}