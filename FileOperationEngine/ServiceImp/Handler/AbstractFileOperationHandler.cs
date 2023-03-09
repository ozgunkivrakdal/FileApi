using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileOperationEngineContract.Model;
using FileOperationEngineContract.Model.Common;
using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service;

using log4net;

using static FileOperationEngineContract.Model.Common.CommonEnums;

namespace FileOperationEngine.ServiceImp
{
    public abstract class AbstractFileOperationHandler : IFileOperationHandler
    {
        protected readonly ILog LOG = LogManager.GetLogger(typeof(AbstractFileOperationHandler));

        private IFileOperationHandler _nextHandler;

        public void SetNextHandler(IFileOperationHandler nextHandler)
        {
            this._nextHandler = nextHandler;
        }
        public FileOperationResModel ProcessRequest(FileOperationReqModel request)
        {
            LOG.Info($"::AbstractFileOperationHandler, handlerType:{this.GetType().Name} started");

            FileOperationResModel response = HandleRequest(request);

            request.addressListOperated = response.operatedList;
            if (_nextHandler == null || !response.success)
            {
                return response;
            }
            LOG.Info($"::AbstractFileOperationHandler, handlerType:{this.GetType().Name} ended");
            return _nextHandler.ProcessRequest(request);
        }

        public abstract FileOperationResModel HandleRequest(FileOperationReqModel request);
    }
}
