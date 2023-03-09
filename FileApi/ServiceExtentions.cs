using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

using FileOperationEngine;
using FileOperationEngine.ServiceImp;
using FileOperationEngine.ServiceImp.Convert;
using FileOperationEngine.ServiceImp.Export;

using FileOperationEngineContract.Service;
using FileOperationEngineContract.Service.Export;

using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

using Microsoft.Extensions.DependencyInjection;

namespace FileApi
{
    public static class ServiceExtentions
    {
        public static void AddFileConverterExecuterServices(this IServiceCollection services)
        {
            services.AddTransient<IFileOperationHandler, FileParseHandler>();
            services.AddTransient<IFileOperationHandler, FileSortHandler>();
            services.AddTransient<IFileOperationHandler, FileFilterHandler>();

            services.AddTransient<IFileOperationChain, FileOperationChain>();
            services.AddTransient<IFileOperationService, FileOperationService>();

            services.AddTransient<IExport, CsvExporter>();
            services.AddTransient<IExport, XmlExporter>();
            
        }

        private static void ConfigLogger(string env)
        {
            try
            {
                XmlDocument log4netConfig = new XmlDocument();
                using (var fs = File.OpenRead($"log4net.{env}.config"))
                {
                    log4netConfig.Load(fs);
                    var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Hierarchy));
                    XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
                }
            }
            catch (Exception ex)
            {

                //NOP
            }
        }
    }
}
