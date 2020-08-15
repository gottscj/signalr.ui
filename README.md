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

The description of the hub methods are extracted from the XML docs. In order for this functionality to work, the documentation
file has to be generated. Add this to your project file:
```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

## Development notes
The UI is under development

### To develop the UI
It is easier to run
```
npm install
npm run serve
```
from **src\SignalR.HubApi.Ui** and use the local configuration

### To run self-hosted from the chat sample
No automated flow is implemented atm. So it is necessary to run:
```
npm install
npm run build
```
from **src\SignalR.HubApi.Ui** then copy the content of the dist folder to **src\SignalR.HubApi\web**
