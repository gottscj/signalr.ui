# signalr.ui

 API endpoint UI and code generator for SignalR Core.
 
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSignalR();
    services.AddHubApi(doc =>
    {
        doc.ApiInfo = new HubApiInfo
        {
            Title = "Chat"
        };
        doc.AddHubDocumentation<ChatHub>("/chat");
    });
}
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseHubApi();
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<ChatHub>("/chat");
    });
}
```