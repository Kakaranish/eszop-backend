using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Utilities.EventBus
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEventHandling(this IApplicationBuilder applicationBuilder, Func<IEventBus, Task> configFunc)
        {
            var eventBus = applicationBuilder.ApplicationServices.GetRequiredService<IEventBus>();
            configFunc(eventBus).GetAwaiter().GetResult();

            return applicationBuilder;
        }
    }
}
