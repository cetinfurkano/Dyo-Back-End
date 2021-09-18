using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken<T>(T entity, string email, List<string> operationClaims) 
            where T: class, IEntity, new();
    }
}
