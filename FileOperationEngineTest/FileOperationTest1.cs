using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileOperationEngine.ServiceImp;
using FileOperationEngine.ServiceImp.Convert;
using FileOperationEngine.ServiceImp.Export;

using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;

using Moq.AutoMock;

using NUnit.Framework;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngineTest
{
    [TestFixture]
    public class FileOperationTest1
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

            if (File.Exists(@"..\..\..\Resources\sample_data_out1.xml"))
            {
                File.Delete(@"..\..\..\Resources\sample_data_out1.xml");
            }
            reqModel.filePath = @"..\..\..\Resources\sample_data.csv";
            reqModel.destFilePath = @"..\..\..\Resources\sample_data_out1.xml";
            reqModel.inputFileExtention = EFileExtention.CSV;
            reqModel.outputFileExtention = EFileExtention.XML;
            Dictionary<string, string> filteringColumns = new Dictionary<string, string>();
            filteringColumns.Add("CityName", "Antalya");
            reqModel.filteringColumns = filteringColumns;
           

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
            Assert.IsTrue(resp.operatedList.Count == 83);
            reqModel.addressListOperated = resp.operatedList;
            TestSort();
        }
        public void TestSort()
        {
            FileOperationResModel resp = sorter.HandleRequest(reqModel);
            Assert.IsNotNull(resp.operatedList);
            Assert.AreEqual(resp.operatedList[0].cityName, "Antalya");
            Assert.IsTrue(resp.operatedList.Count == 83);
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
