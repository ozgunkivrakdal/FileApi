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
            services.AddSingleton<IFileOperationHandler, FileParseHandler>();
            services.AddSingleton<IFileOperationHandler, FileSortHandler>();
            services.AddSingleton<IFileOperationHandler, FileFilterHandler>();

            services.AddSingleton<IFileOperationChain, FileOperationChain>();
            services.AddSingleton<IFileOperationService, FileOperationService>();

            services.AddSingleton<IExport, CsvExporter>();
            services.AddSingleton<IExport, XmlExporter>();
            
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
