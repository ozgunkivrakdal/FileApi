using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngineContract.Model.Base;
using FileOperationEngineContract.Model.Common;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngineContract.Model.Operation
{
    public class FileOperationReqModel : BaseRequestModel
    {
        public List<AddressInfoModel> addressListOperated { get; set; }
        public EFileExtention inputFileExtention { get; set; }
        public EFileExtention outputFileExtention { get; set; }
        public string filePath { get; set; }
        public string destFilePath { get; set; }
        public Dictionary<string, CommonEnums.ESortingType> sortingColumns { get; set; }
        public Dictionary<string, string> filteringColumns { get; set; }
        public FileOperationReqModel(string uuid, TimeSpan timeout) : base(uuid, timeout)
        {
            addressListOperated = new List<AddressInfoModel>();

        }
    }
    }
