using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SignalR.HubApi
{
    internal class HubApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HubApiConfiguration _hubApiConfiguration;

        public HubApiMiddleware(RequestDelegate next, HubApiConfiguration hubApiConfiguration)
        {
            _next = next;
            _hubApiConfiguration = hubApiConfiguration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "GET")
            {
                await _next(context);
                return;
            }

            var referer = SanitizeReferer(context.Request.GetTypedHeaders().Referer?.AbsolutePath ?? "");
            var requestPath = referer + context.Request.Path;
            var file = HubFileReader.GetFile(requestPath, _hubApiConfiguration);
            if (!file.Exist)
            {
                await _next.Invoke(context);
                return;
            }

            context.Response.ContentType = MimeTypeProvider.Get(file.Extension);
            context.Response.StatusCode = 200;
            await context.Response.Body.WriteAsync(file.Buffer, 0, file.Buffer.Length);
        }
        private static string SanitizeReferer(string referer)
        {
            if (referer.EndsWith("/"))
            {
                return referer.Substring(0, referer.Length - 1);
            }
            return referer;
        }
    }
}