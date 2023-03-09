using System;
using System.Collections.Generic;
using System.Text;

namespace FileOperationEngineContract.Model.Common
{
    public class CommonEnums
    {
        public enum EFileExtention
        {
            ALL = -1,
            UNKNOWN = 0,
            CSV = 1,
            XML = 2
        }

        public enum EOperationType
        {
            ALL = -1,
            UNKNOWN = 0,
            SORT = 1,
            FILTER = 2,
            CONVERT  = 3, 
        }
        public enum ESortingType
        {
            ALL = -1,
            UNKNOWN = 0,
            ASC = 1,
            DESC = 2
        }
        
    }
}
