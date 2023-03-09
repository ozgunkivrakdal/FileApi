using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FileApiContract.Model.FileConvert;

using FileOperationEngineContract.Model.Operation;

namespace FileApi.Helper
{
    public static class Converters
    {
        public static FileOperationReqModel ConvertOperationRequest(FileOperationRequestModel req) 
        {
            FileOperationReqModel model = new FileOperationReqModel(req.uuid, req.timeout);
            model.filePath = req.filePath;
            model.destFilePath = req.destFilePath;
            model.filteringColumns = req.filteringColumns;
            model.sortingColumns = req.sortingColumns;

            return model;



        }

        public  static FileOperationResponseModel ConvertOperationResponse(FileOperationResModel response)
        {
            FileOperationResponseModel model = new FileOperationResponseModel();
            model.success = response.success;
            model.result = response.result;

            return model;

        }
    }
}
