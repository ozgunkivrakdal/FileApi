using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngineContract.Model.Base;

namespace FileOperationEngineContract.Model.Operation
{
    public class FileOperationResModel : BaseResponseModel
    {
        public List<AddressInfoModel> operatedList  { get; set; }
    }
}
