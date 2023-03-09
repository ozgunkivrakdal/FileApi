using System;
using System.Collections.Generic;
using System.Text;

using FileApiContract.Model;
using FileApiContract.Model.FileConvert;

namespace FileApiContract.Service
{
    public interface IFileApiService
    {
        FileOperationResponseModel Operate(FileOperationRequestModel request);

    }
}
