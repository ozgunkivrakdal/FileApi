using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Operation;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngineContract.Service.Filter
{
    public interface IFilter
    {
        FileOperationResModel Filter(List<AddressInfoModel> addressList, Dictionary<string, string> filteringColumns);

    }
}
