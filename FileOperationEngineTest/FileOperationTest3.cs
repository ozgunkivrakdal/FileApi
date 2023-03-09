using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using FileOperationEngine.ServiceImp;
using FileOperationEngine.ServiceImp.Convert;
using FileOperationEngine.ServiceImp.Export;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service;
using FileOperationEngineContract.Service.Export;

using Moq;
using Moq.AutoMock;

using NUnit.Framework;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngineTest
{
    [TestFixture]
    public class TestFileOperation
    {
        private FileParseHandler parser;
        private FileFilterHandler filter;
        private FileSortHandler sorter;
        private CsvExporter export;
        private FileOperationReqModel reqModel;

        [SetUp]
        public void Init()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            reqModel = new FileOperationReqModel(null, TimeSpan.FromSeconds(60));

            if (File.Exists(@"..\..\..\Resources\sample_data_out3.csv"))
            {
                File.Delete(@"..\..\..\Resources\sample_data_out3.csv");
            }
            reqModel.filePath = @"..\..\..\Resources\sample_data.xml";
            reqModel.destFilePath = @"..\..\..\Resources\sample_data_out3.csv";
            reqModel.inputFileExtention = EFileExtention.XML;
            reqModel.outputFileExtention = EFileExtention.CSV;
            Dictionary<string, string> filteringColumns = new Dictionary<string, string>();
            filteringColumns.Add("CityName", "Ankara");
            reqModel.filteringColumns = filteringColumns;
            Dictionary<string, CommonEnums.ESortingType> sortingColumns = new Dictionary<string, CommonEnums.ESortingType>();
            sortingColumns.Add("ZipCode", CommonEnums.ESortingType.DESC);
            reqModel.sortingColumns = sortingColumns;

            var mocker = new AutoMocker();

            parser = mocker.CreateInstance<FileParseHandler>();
            sorter = mocker.CreateInstance<FileSortHandler>();
            filter = mocker.CreateInstance<FileFilterHandler>();
            export = mocker.CreateInstance<CsvExporter>();
            
        }

        [Test]
        public void TestParse()
        {
            FileOperationResModel resp = parser.HandleRequest(reqModel);
            Assert.IsNotNull(resp.operatedList);
            Assert.IsTrue(resp.operatedList.Count == 3232);
            reqModel.addressListOperated = resp.operatedList;

            TestFilter();
        }
       
        public void TestFilter()
        {
            FileOperationResModel resp = filter.HandleRequest(reqModel);
            Assert.IsNotNull(resp.operatedList);
            Assert.IsTrue(resp.operatedList.Count == 121);
            reqModel.addressListOperated = resp.operatedList;
            TestSort();
        }
        public void TestSort()
        {
            FileOperationResModel resp = sorter.HandleRequest(reqModel);
            Assert.IsNotNull(resp.operatedList);
            Assert.AreEqual(resp.operatedList[0].districtName, "Kazan");
            Assert.IsTrue(resp.operatedList.Count == 121);
            reqModel.addressListOperated = resp.operatedList;
            TestExport();

        }

        public void TestExport()
        {
            export.ExportFile(reqModel.addressListOperated, reqModel.destFilePath);
            Assert.That(reqModel.destFilePath, Does.Exist);
        }
    }
}