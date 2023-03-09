using System;
using System.Collections.Generic;
using System.Text;

namespace FileOperationEngineContract.Service
{
    public interface IFileOperationChain
    {
        IFileOperationHandler GetChain();
    }
}
