using System;
using System.Threading.Tasks;


using log4net;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace FileApi.Middleware
{
    public class ExceptionMiddleware
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(ExceptionMiddleware));

        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            LOG.Fatal(exception);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(exception.ToString());
        }
    }
}
