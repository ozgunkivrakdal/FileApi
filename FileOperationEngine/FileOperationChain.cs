using System;
using System.Collections.Generic;
using System.Text;

using FileOperationEngine.ServiceImp;
using FileOperationEngine.ServiceImp.Convert;

using FileOperationEngineContract.Model.Operation;
using FileOperationEngineContract.Service;

namespace FileOperationEngine
{
    public class FileOperationChain : IFileOperationChain
    {

		public IFileOperationHandler GetChain()
		{
			//chain creation is done according to proceed with .csv files to filter and sort. After manupulation is succeed,
			//last convert chain  converts file demanded output type.


            // initialize the chain
            IFileOperationHandler parse = new FileParseHandler();
			IFileOperationHandler filter = new FileFilterHandler();
			IFileOperationHandler sort = new FileSortHandler();


			// set the chain of responsibility
			parse.SetNextHandler(filter);
			filter.SetNextHandler(sort);
            return parse;
		}
    }
}
