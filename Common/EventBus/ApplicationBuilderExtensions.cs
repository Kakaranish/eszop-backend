using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Common.EventBus
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
