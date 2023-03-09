using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;


using FileOperationEngine.Model;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service.Convert;

using log4net;

namespace FileOperationEngine.ServiceImp.Convert
{
    public class FileCsvParser : IParse
    {
        private readonly ILog LOG = LogManager.GetLogger(typeof(FileCsvParser));

        public FileOperationResModel Parse(string path)
        {
            FileOperationResModel response = new FileOperationResModel();
            List<AddressInfoModel> commonModelList = new List<AddressInfoModel>();
            try
            {
                commonModelList = ParseCommon(path);
            }
            catch (Exception e)
            {

                LOG.Error("::FileCsvParser", e);
                response.success = false;
                response.result = e.Message;
                return response;
            }

            response.success = true;
            response.operatedList = commonModelList;

            return response;

        }

        private static List<AddressInfoModel> ParseCommon(string path)
        {
            List<AddressInfoModelCsv> addressInfoCsv = File.ReadAllLines(path)
                                             .Skip(1)
                                             .Select(v => AddressInfoModelCsv.FromCsv(v))
                                             .ToList();


            List<AddressInfoModel> commonModelList = new List<AddressInfoModel>();
            foreach (AddressInfoModelCsv addressInfo in addressInfoCsv)
            {
                AddressInfoModel address = new AddressInfoModel(addressInfo.cityName, addressInfo.cityCode, addressInfo.districtName, addressInfo.zipCode);

                commonModelList.Add(address);
            }

            return commonModelList;
        }
    }
}
