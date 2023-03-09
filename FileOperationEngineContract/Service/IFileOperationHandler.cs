using FileOperationEngineContract.Model.Operation;

namespace FileOperationEngineContract.Service
{
    public interface IFileOperationHandler
    {
        FileOperationResModel ProcessRequest(FileOperationReqModel request);
        void SetNextHandler(IFileOperationHandler nextHandler);
    }
}