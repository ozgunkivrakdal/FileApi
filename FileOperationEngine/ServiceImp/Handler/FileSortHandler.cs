using System;
using System.Collections.Generic;
using FileOperationEngine.ServiceImp.Sort;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service.Sort;

namespace FileOperationEngine.ServiceImp
{
    public class FileSortHandler : AbstractFileOperationHandler
    {
        public  override FileOperationResModel HandleRequest(FileOperationReqModel request)
        {
            FileOperationResModel response = new FileOperationResModel();

            if (request.sortingColumns != null && request.sortingColumns.Count > 0)
            {
                ISort sorter = new SortOperator();

                return sorter.Sort(request.addressListOperated, request.sortingColumns);
            }
            response.success = true;
            response.operatedList = request.addressListOperated;
            return response;
        }
    }
}
