using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service.Filter;

using log4net;

namespace FileOperationEngine.ServiceImp.Filter
{
    public class FilterOperator : IFilter
    {
        private readonly ILog LOG = LogManager.GetLogger(typeof(FilterOperator));

        public FileOperationResModel Filter(List<AddressInfoModel> addressList, Dictionary<string, string> filteringColumns)
        {
            FileOperationResModel response = new FileOperationResModel();
            string filterStatement = null;

            foreach (KeyValuePair<string, string> column in filteringColumns)
            {
                filterStatement += column.Key + " == " + "\"" + column.Value+ "\"";
                filterStatement += ", ";
            }
            filterStatement = filterStatement.TrimEnd(' ').TrimEnd(',');

            try
            {
                response.operatedList = addressList.AsQueryable().Where(filterStatement).ToList();
            }
            catch (Exception e)
            {

                LOG.Error("::FilterOperator", e);
                response.success = false;
                response.result = e.Message;
                return response;
            }
            response.success = true;

            return response;
        }
    }
}
