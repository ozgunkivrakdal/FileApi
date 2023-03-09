using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service.Filter;
using FileOperationEngineContract.Service.Sort;

using log4net;

namespace FileOperationEngine.ServiceImp.Sort
{
    public class SortOperator : ISort
    {
        private readonly ILog LOG = LogManager.GetLogger(typeof(SortOperator));

        public FileOperationResModel Sort(List<AddressInfoModel> addressList, Dictionary<string, CommonEnums.ESortingType> sortingColumns)
        {
            FileOperationResModel response = new FileOperationResModel();
            string orderByStatement = null;

            foreach (KeyValuePair<string, CommonEnums.ESortingType> column in sortingColumns)
            {
                orderByStatement += column.Key;

                if (column.Value.Equals(CommonEnums.ESortingType.DESC))
                    orderByStatement += " desc";
                orderByStatement += ", ";
            }
            orderByStatement = orderByStatement.TrimEnd(' ').TrimEnd(',');

            try
            {
                response.operatedList = addressList.AsQueryable().OrderBy(orderByStatement).ToList();
            }
            catch (Exception e)
            {
                LOG.Error("::SortOperator", e);
                response.success = false;
                response.result = e.Message;
                return response;
            }

            response.success = true;
            return response;
        }
    }
}
