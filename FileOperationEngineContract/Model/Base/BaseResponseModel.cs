

namespace FileOperationEngineContract.Model.Base
{
    public class BaseResponseModel
    {
        public string uuid { get; set; }
        public string result { get; set; }

        public string code { get; set; }
        public int id { get; set; }
        public bool success { get; set; }

        public BaseResponseModel() { }

        public BaseResponseModel(string uuid, string result, bool success)
        {
            this.uuid = uuid;
            this.result = result;
            this.success = false;

        }
    }
}
