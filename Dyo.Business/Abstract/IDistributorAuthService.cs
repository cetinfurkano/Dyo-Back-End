using Dyo.Core.Utilities.Communication;
using Dyo.Core.Utilities.Security.JWT;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Abstract
{
    public interface IDistributorAuthService
    {
        Task<OperationResponse<Distributor>> RegisterAsync(Distributor distributor, string password);
        Task<OperationResponse<Distributor>> LoginAsync(DistributorForLoginDto distributor);
        OperationResponse<AccessToken> CreateAccessToken(Distributor distributor);
        Task<OperationResponse<Distributor>> UserExistsAsync(string email);
        Task<OperationResponse<Distributor>> UpdateAsync(Distributor distributor, string password);

    }
}
