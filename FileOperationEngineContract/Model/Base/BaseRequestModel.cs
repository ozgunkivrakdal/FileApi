using System;
namespace FileOperationEngineContract.Model.Base
{
        public class BaseRequestModel
        {   

            public string uuid { get; set; }
            public TimeSpan timeout { get; set; }

        public BaseRequestModel(string uuid, TimeSpan timeout)
        {
            this.uuid = uuid;
            this.timeout = timeout;
        }
    }
}
