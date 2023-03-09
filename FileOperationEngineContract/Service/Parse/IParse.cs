using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngineContract.Service.Convert
{
    public interface IParse
    {
        FileOperationResModel Parse(string path);

        
    }
}
