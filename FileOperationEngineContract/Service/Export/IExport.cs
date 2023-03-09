using System;
using System.Collections.Generic;
using System.Text;

using FileOperationEngineContract.Model;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngineContract.Service.Export
{
    public interface IExport
    {
        EFileExtention GetExportType();
        void ExportFile(List<AddressInfoModel> addressInfoList, string destFilePath);
    }
}
