using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Common.ErrorHandling.ActionResults
{
    public class ErrorActionResult : IActionResult
    {
        [JsonIgnore]
        public int StatusCode { get; set; } = 400;

        public string Message { get; }
        public IEnumerable<Error> Errors { get; set; }
        public IEnumerable<PropertyError> PropertyErrors { get; set; }

        public ErrorActionResult(string message, int statusCode) : this(message)
        {
            StatusCode = statusCode;
        }

        public ErrorActionResult(string message)
        {
            Message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            context.HttpContext.Response.ContentType = "application/json";

            var serialized = JsonConvert.SerializeObject(
                value: this,
                formatting: Formatting.None,
                settings: new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var serializedAsBytes = Encoding.UTF8.GetBytes(serialized);
            await context.HttpContext.Response.Body.WriteAsync(serializedAsBytes);
        }
    }
}
