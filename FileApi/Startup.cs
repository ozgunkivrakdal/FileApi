using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

using FileApi.Json;
using FileApi.Middleware;

using log4net;
using log4net.Config;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileApi
{
    public class Startup
    {
        private readonly ILog LOG = LogManager.GetLogger(typeof(Startup));

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {


            AddLogger(_configuration["ASPNETCORE_ENVIRONMENT"]);


            services.AddHttpClient();
            services.AddFileConverterExecuterServices();



            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter()));

            LOG.Info($"::ASPNETCORE_ENVIRONMENT:{_configuration["ASPNETCORE_ENVIRONMENT"]}");
            LOG.Info($"::SERVER_NAME:{_configuration["SERVER_NAME"]}");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName.Equals("LOCAL"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            if ("DEV".Equals(_configuration["ASPNETCORE_ENVIRONMENT"]))
                app.UseMiddleware<ProxyMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddLogger(string env)
        {
            try
            {
                XmlDocument log4netConfig = new XmlDocument();
                using (var fs = File.OpenRead($"log4net.{env}.config"))
                {
                    log4netConfig.Load(fs);
                    var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                    XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
                }
            }
            catch (Exception)
            {
                //NOP
            }
        }


    }
  
}

