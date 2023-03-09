using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using FileOperationEngine.Model;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service.Convert;

using log4net;

namespace FileOperationEngine.ServiceImp.Convert
{
    public class FileXmlParser : IParse
    {
        private readonly ILog LOG = LogManager.GetLogger(typeof(FileXmlParser));

        public FileOperationResModel Parse(string path)
        {
            FileOperationResModel response = new FileOperationResModel();
            List<AddressInfoModel> commonList = new List<AddressInfoModel>();
            try
            {
                commonList = ParseCommon(path);
            }
            catch (Exception e)
            {

                LOG.Error("::FileXmlParser", e);
                response.success = false;
                response.result = e.Message;
                return response;
            }
            response.success = true;
            response.operatedList = commonList;
            return response;

        }

        private static List<AddressInfoModel> ParseCommon(string path)
        {
            AddressInfoModelXml xml = new AddressInfoModelXml();

            XmlSerializer ser = new XmlSerializer(typeof(AddressInfoModelXml));

            using (StreamReader sr = new StreamReader(path))
            {
                xml = (AddressInfoModelXml)ser.Deserialize(sr);
            }
            List<AddressInfoModel> commonList = xml.cities.SelectMany(d => d.districts.SelectMany(z => z.Zipcodes.Select(a => new AddressInfoModel(
                d.Name,
                d.Code,
                z.Name,
                a.Code)))).ToList();
            return commonList;
        }

    }
}
