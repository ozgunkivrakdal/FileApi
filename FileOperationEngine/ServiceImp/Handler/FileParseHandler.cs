using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service;
using FileOperationEngineContract.Service.Convert;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngine.ServiceImp.Convert
{
    public class FileParseHandler : AbstractFileOperationHandler
    {
  
        public override FileOperationResModel HandleRequest(FileOperationReqModel request)
        {
            FileOperationResModel response = new FileOperationResModel();
            IParse parser = null;
                switch (request.inputFileExtention)
                {
                    case CommonEnums.EFileExtention.CSV:
                         parser = new FileCsvParser();
                    break;
                    case CommonEnums.EFileExtention.XML:
                        parser = new FileXmlParser();
                        break;
                default:
                    response.result = "Input file extention is not known";
                    return response;

                }

                if (parser != null)
                    return parser.Parse(request.filePath);


            response.operatedList = request.addressListOperated;
            return response;
        }
    }
}
