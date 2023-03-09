using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngineContract.Model.Operation;

namespace FileOperationEngineContract.Service
{
    public interface IFileOperationService
    {
        FileOperationResModel Operate(FileOperationReqModel fileOperationReqModel);
    }
}
