using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngine.ServiceImp.Convert;
using FileOperationEngine.ServiceImp.Filter;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service;
using FileOperationEngineContract.Service.Convert;
using FileOperationEngineContract.Service.Filter;

namespace FileOperationEngine.ServiceImp
{
    public class FileFilterHandler : AbstractFileOperationHandler
    {
        public  override FileOperationResModel HandleRequest(FileOperationReqModel request)
        {
            FileOperationResModel response = new FileOperationResModel();
            if (request.filteringColumns != null && request.filteringColumns.Count > 0)
            {
                IFilter filter = new FilterOperator();

                return filter.Filter(request.addressListOperated, request.filteringColumns);
            }
            response.success = true;
            response.operatedList = request.addressListOperated;
            return response;
        }
    }
}
