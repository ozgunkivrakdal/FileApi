using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Service.Export;

namespace FileOperationEngine.ServiceImp.Export
{
    public class CsvExporter : IExport
    {
        public void ExportFile(List<AddressInfoModel> addressInfoList, string destFilePath)
        {
            var sb = new StringBuilder();
            var info = typeof(AddressInfoModel).GetProperties();
            var header = "";

            DeletePreviousFile(destFilePath);

            if (!File.Exists(destFilePath))
            {
                var file = File.Create(destFilePath);
                file.Close();
                foreach (var prop in typeof(AddressInfoModel).GetProperties())
                {
                    header += prop.Name + ", ";
                }
                header = header.Substring(0, header.Length - 2);
                sb.AppendLine(header);
                TextWriter sw = new StreamWriter(destFilePath, true);
                sw.Write(sb.ToString());
                sw.Close();
            }

            foreach (var obj in addressInfoList)
            {
                sb = new StringBuilder();
                var line = "";
                foreach (var prop in info)
                {
                    line += prop.GetValue(obj, null) + ", ";
                }
                line = line.Substring(0, line.Length - 2);
                //Encoding assumption !!
                TextWriter sw = new StreamWriter(destFilePath, true, Encoding.GetEncoding("Windows-1252"));
                sb.AppendLine(line);
                sw.Write(sb.ToString());
                sw.Close();

            }

        }

        private static void DeletePreviousFile(string destFilePath)
        {
            if (File.Exists(destFilePath))
            {
                File.Delete(destFilePath);
            }
        }

        public CommonEnums.EFileExtention GetExportType()
        {
            return CommonEnums.EFileExtention.CSV;
        }
    }
}
