using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Common.Utilities.ErrorHandling.ActionResults
{
    public class ValidationFailureActionResult : IActionResult
    {
        [JsonProperty(Order = 1)]
        public const string Message = "Validation failed";

        [JsonProperty(PropertyName = "PropertiesErrors", Order = 2)]
        public IEnumerable<PropertyErrors> PropertyErrorsList { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 400;
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
