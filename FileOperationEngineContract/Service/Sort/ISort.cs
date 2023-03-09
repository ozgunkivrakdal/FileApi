using System;
using System.Collections.Generic;
using System.Text;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Operation;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngineContract.Service.Sort
{
    public interface ISort
    {
      FileOperationResModel Sort(List<AddressInfoModel> addressList, Dictionary<string, ESortingType> sortingColumns);

    }
}
