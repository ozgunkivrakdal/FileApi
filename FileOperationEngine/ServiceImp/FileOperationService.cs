using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service;
using FileOperationEngineContract.Service.Export;

using log4net;

using Newtonsoft.Json;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngine.ServiceImp
{
    public class FileOperationService : IFileOperationService
    {
        protected readonly ILog LOG = LogManager.GetLogger(typeof(FileOperationService));

        private IFileOperationChain fileOperationChain;
        private Dictionary<EFileExtention, IExport> exportServiceMap = new Dictionary<EFileExtention, IExport>();

        public FileOperationService(IFileOperationChain fileOperationChain, IEnumerable<IExport> exportServices)
        {
            this.fileOperationChain = fileOperationChain;
            foreach (IExport exportService in exportServices)
                exportServiceMap.Add(exportService.GetExportType(), exportService);
        }
        public FileOperationResModel Operate(FileOperationReqModel request)
        {
            FileOperationResModel response = new FileOperationResModel();
            if (request.filePath == null || request.destFilePath == null)
            {
                response.success = false;
                response.result = "Input or Output file is missing";
                return response;
            }
            SetFileExtentions(request);
            //create chain and process file
            var chain = fileOperationChain.GetChain();
            
            LOG.Info($"::FileOperationService, request:{JsonConvert.SerializeObject(request)}");
                response = chain.ProcessRequest(request);
            if (!response.success) 
            {
                LOG.Error($"::FileOperationService, response:{JsonConvert.SerializeObject(response)}");
                return response;
            }
            LOG.Info($"::FileOperationService, response:{JsonConvert.SerializeObject(response)}");

            // export
            LOG.Info($"::FileOperationService, export file to {request.destFilePath}");
            
            try
            {
                IExport exportService = exportServiceMap[request.outputFileExtention];
                exportService.ExportFile(response.operatedList, request.destFilePath);
            }
            catch (Exception e)
            {
                LOG.Error("::FileOperationService Exporting is failed", e);
                response.success = false;
                response.result = e.Message;
                return response;
            }
            
            LOG.Info($"::FileOperationService, file exported to {request.destFilePath}");

            response.result = "operated";
            response.success = true;

            return response;
        }

        private static void SetFileExtentions(FileOperationReqModel request)
        {
            EFileExtention inputFile;
            Enum.TryParse<CommonEnums.EFileExtention>(Path.GetExtension(request.filePath).TrimStart('.').ToUpper(), out inputFile);
            EFileExtention outFile;
            Enum.TryParse<CommonEnums.EFileExtention>(Path.GetExtension(request.destFilePath).TrimStart('.').ToUpper(), out outFile);

            request.inputFileExtention = inputFile;
            request.outputFileExtention = outFile;
        }
    }
}
