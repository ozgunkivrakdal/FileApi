using System;
using System.IO;
using System.Net;
using System.Net.Http;

using FileApi.Helper;

using FileApiContract.Model.FileConvert;

using FileOperationEngine;
using FileOperationEngine.ServiceImp;

using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service;

using log4net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FileApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileOperationController
    {
        private readonly ILog LOG = LogManager.GetLogger(typeof(FileOperationController));

        private readonly IFileOperationService fileOperationService;

        public FileOperationController(IFileOperationService fileOperationService)
        {
            this.fileOperationService = fileOperationService;
        }


        [HttpPost("Operate")]
        public FileOperationResponseModel Operate(FileOperationRequestModel req)
        {

            FileOperationReqModel fileOperationReqModel = Converters.ConvertOperationRequest(req);

            FileOperationResModel response = fileOperationService.Operate(fileOperationReqModel);
            return Converters.ConvertOperationResponse(response);
        }


    }
}
