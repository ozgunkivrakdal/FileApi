using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

using Microsoft.AspNetCore.Http;


namespace FileApi.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly ILog LOG = LogManager.GetLogger(typeof(AuthorizationMiddleware));

        private readonly RequestDelegate next;
        private readonly Dictionary<string, bool> excludedPaths;
        private Dictionary<string, bool> passwords;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            this.next = next;

            this.excludedPaths = new Dictionary<string, bool>();
            this.excludedPaths["/api/healthcheck"] = true;
            this.excludedPaths["/api/version"] = true;

            this.passwords = new Dictionary<string, bool>();
            this.passwords["c28pMc*2Wgab5rWP"] = true; //2021.09.09
        }

        public async Task Invoke(HttpContext context)
        {
            if (excludedPaths.ContainsKey(context.Request.Path.ToString().ToLower()))
            {
                await next.Invoke(context);
                return;
            }

            string authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null)
            {
                try
                {
                    string encodedToken = authHeader.Replace("Basic ", "");
                    string plainToken = Encoding.UTF8.GetString(Convert.FromBase64String(encodedToken));

                    string[] tokens = plainToken.Split(new[] { ':' }, 2);
                    string[] pars = tokens[0].Split('_');
                    string password = tokens[1];

                    int userId = Convert.ToInt32(pars[0]);
                    int timezoneId = Convert.ToInt32(pars[1]);
                    

                    if (passwords.ContainsKey(password)
                        && userId > 0 && timezoneId > 0)
                    {
                        context.Items["USER_ID"] = userId;
                        context.Items["TIMEZONE_ID"] = timezoneId;

                        await next.Invoke(context);
                        return;
                    }
                }
                catch (Exception e)
                {
                    LOG.Fatal($"authorization:{authHeader} not valid", e);
                }
            }

            //Unauthorized
            context.Response.StatusCode = 401;
            return;
        }
    }
}
