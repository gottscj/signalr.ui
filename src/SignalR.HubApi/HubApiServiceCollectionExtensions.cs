using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SignalR.HubApi
{
    public static class HubApiServiceCollectionExtensions
    {
        
        public static IServiceCollection AddHubApi(this IServiceCollection serviceCollection, Action<HubApiConfiguration> configure)
        {
            var hubApiDoc = new HubApiConfiguration();
            configure?.Invoke(hubApiDoc);
            
            serviceCollection.AddSingleton(hubApiDoc);
            
            return serviceCollection;
        }
        public static IApplicationBuilder UseHubApi(this IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<HubApiConfiguration>();
            if (config == null)
            {
                config = new HubApiConfiguration();
            }

            app.UseMiddleware<HubApiMiddleware>(config);
            return app;
        }
    }
}