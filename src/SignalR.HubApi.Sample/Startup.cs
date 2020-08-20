using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SignalR.HubApi.Sample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddHubApi(doc =>
            {
                doc.ApiInfo = new HubApiInfo
                {
                    Title = "Chat",
                    Description = "Sample chap api",
                    Version = "v1.0",
                    Contact = new HubApiContact
                    {
                        Name = "The Dude Li Du",
                        Email = "dude@lidu.com",
                        Url = "xxx.dudelidu.xxx"
                    }
                };
                doc.AddHubDocumentation<ChatHub>("/chat");
            });

            // For dev purposes only
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:8080") // URL served by UI in dev
                        .AllowCredentials();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // For dev purposes only
            app.UseCors("AllowAll");

            app.UseHubApi();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}