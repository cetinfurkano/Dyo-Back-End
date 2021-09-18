using Dyo.Core.Utilities.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Core.Utilities.Business
{
    public class BusinessRules
    {
        public static OperationResponse<T> Run<T>(params OperationResponse<T>[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                }
            }
            return null;
        }
    }
}
