using System;
using System.Collections.Generic;
using System.Text;

using FileApiContract.Model;
using FileApiContract.Model.FileConvert;
using FileApiContract.Service;

namespace FileApiContract.Proxy
{
    public class FileApiService : BaseService, IFileApiService
    {

        public FileApiService(string endPointUrl) : base(endPointUrl, " ")
        {

        }

        public FileOperationResponseModel Operate(FileOperationRequestModel request)
        {
            return Post<FileOperationResponseModel>(request, "Operate");
        }
    }
}
