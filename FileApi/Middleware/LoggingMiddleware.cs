using System.IO;
using System.Text;
using System.Threading.Tasks;

using log4net;

using Microsoft.AspNetCore.Http;

namespace FileApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly ILog LOG = LogManager.GetLogger(typeof(LoggingMiddleware));

        private readonly RequestDelegate next;

        public LoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            var request = await FormatRequest(context.Request);
            if (request != null)
                LOG.Info($"::Invoke request:{request}");

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await next(context);

                var response = await FormatResponse(context.Response);
                if (response != null)
                    LOG.Info($"::Invoke response:{response}");

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            if (request.Path.ToString().ToLower().Contains("healthcheck"))
                return null;

            string content = null;
            HttpRequestRewindExtensions.EnableBuffering(request);
            using (StreamReader reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true))
            {
                content = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }
            return $"{request.Scheme}://{request.Host}{request.Path}{(request.QueryString.ToString().Length > 0 ? "?" : "")} {content}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            if (response.HttpContext.Request.Path.ToString().ToLower().Contains("healthcheck"))
                return null;

            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            HttpRequest request = response.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.Path}{(request.QueryString.ToString().Length > 0 ? "?" : "")} {response.StatusCode}:{text}";
        }
    }
}
