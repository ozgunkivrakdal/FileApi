using System;
using System.Collections.Generic;
using System.Text;

using FileOperationEngineContract.Model.Base;
using FileOperationEngineContract.Model.Common;

using Microsoft.AspNetCore.Http;


namespace FileApiContract.Model.FileConvert
{
    public class FileOperationRequestModel : BaseRequestModel
    {
        public string filePath { get; set; }
        public string destFilePath { get; set; }
        public Dictionary<string, CommonEnums.ESortingType> sortingColumns { get; set; }
        public Dictionary<string,string> filteringColumns { get; set; }
        public FileOperationRequestModel(string uuid, TimeSpan timeout) : base(uuid, timeout)
        {

        }
    }
}
