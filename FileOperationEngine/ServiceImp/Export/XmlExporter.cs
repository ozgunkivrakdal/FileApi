using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using FileOperationEngine.Model;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Service.Export;

namespace FileOperationEngine.ServiceImp.Export
{
    public class XmlExporter : IExport
    {
        public void ExportFile(List<AddressInfoModel> addressInfoList, string destFilePath)
        {
            AddressInfoModelXml xmlModel = new AddressInfoModelXml();

            ConvertToXmlModel(addressInfoList, xmlModel);
            DeletePreviousFile(destFilePath);
            FileInfo file = new FileInfo(destFilePath);
            StreamWriter sw = file.AppendText();

            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(AddressInfoModelXml));
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (var writer = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(writer, xmlModel, emptyNamespaces);

            }
            sw.Close();
        }

        private static void DeletePreviousFile(string destFilePath)
        {
            if (File.Exists(destFilePath))
            {
                File.Delete(destFilePath);
            }
        }

        private static void ConvertToXmlModel(List<AddressInfoModel> addressInfoList, AddressInfoModelXml xmlModel)
        {
            //flat object to nested object
            var nested = addressInfoList.GroupBy(r => r.cityName).Select(g =>
                                                new
                                                {
                                                    name = g.Key,
                                                    code = g.First().cityCode,
                                                    districts = g.GroupBy(r => r.districtName).Select(cr =>
                                                     new
                                                     {
                                                         districtName = cr.Key,
                                                         zipcodes = cr.GroupBy(z => z.zipCode).Select(zc =>
                                                          new
                                                          {
                                                              zipCode = zc.Key
                                                          }).ToList()
                                                     }).ToList()
                                                }).ToList();

            //nested object to xmlModel
            List<AddressInfoModelXml.City> cities = new List<AddressInfoModelXml.City>();
            foreach (var city in nested)
            {
                AddressInfoModelXml.City cityModel = new AddressInfoModelXml.City();
                cityModel.Name = city.name;
                cityModel.Code = city.code;
                List<AddressInfoModelXml.District> districts = new List<AddressInfoModelXml.District>();
                foreach (var district in city.districts)
                {
                    AddressInfoModelXml.District districtModel = new AddressInfoModelXml.District();
                    districtModel.Name = district.districtName;
                    List<AddressInfoModelXml.Zip> zipCodes = new List<AddressInfoModelXml.Zip>();
                    foreach (var zipCode in district.zipcodes)
                    {
                        AddressInfoModelXml.Zip zipModel = new AddressInfoModelXml.Zip();
                        zipModel.Code = zipCode.zipCode;
                        zipCodes.Add(zipModel);
                    }
                    districtModel.Zipcodes = zipCodes;
                    districts.Add(districtModel);
                }
                cityModel.districts = districts;
                cities.Add(cityModel);
            }
            xmlModel.cities = cities;
        }

        public CommonEnums.EFileExtention GetExportType()
        {
            return CommonEnums.EFileExtention.XML;
        }
    }
}
