using Dyo.Business.Abstract;
using Dyo.Core.Utilities.Communication;
using Dyo.Core.Utilities.Security.Hashing;
using Dyo.Core.Utilities.Security.JWT;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Concrete.Managers
{
    public class DistributorAuthManager : IDistributorAuthService
    {
        private readonly IDistributorService _distributorService;
        private readonly ITokenHelper _tokenHelper;
        public DistributorAuthManager(IDistributorService distributorService, ITokenHelper tokenHelper)
        {
            _distributorService = distributorService;
            _tokenHelper = tokenHelper;
        }

        public OperationResponse<AccessToken> CreateAccessToken(Distributor distributor)
        {
            var roles = distributor.Roles;
            var accessToken = _tokenHelper.CreateToken(distributor, distributor.Email, roles);
            return OperationResponse<AccessToken>.CreateSuccesResponse(accessToken);
        }

        public async Task<OperationResponse<Distributor>> LoginAsync(DistributorForLoginDto distributorDto)
        {
            var distributorToCheck = await _distributorService.GetByFilterAsync(d => d.Email == distributorDto.Email);
            if (!distributorToCheck.Success)
            {
                return OperationResponse<Distributor>.CreateFailure("Email ya da parola hatalı!");
            }

            if (!HashingHelper.VerifyPasswordHash(distributorDto.Password, distributorToCheck.Resource.PasswordHash, distributorToCheck.Resource.PasswordSalt))
            {
                return OperationResponse<Distributor>.CreateFailure("Email ya da parola hatalı!");
            }

            return distributorToCheck;
        }

        public async Task<OperationResponse<Distributor>> RegisterAsync(Distributor distributor, string password)
        {
            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            distributor.PasswordHash = passwordHash;
            distributor.PasswordSalt = passwordSalt;

            var result = await _distributorService.AddAsync(distributor);

            return result;
        }

        public async Task<OperationResponse<Distributor>> UserExistsAsync(string email)
        {
            var result = await _distributorService.GetByFilterAsync(t => t.Email == email);
            if (result.Success)
            {
                return OperationResponse<Distributor>.CreateFailure("Kullanıcı mevcut");
            }
            return OperationResponse<Distributor>.CreateSuccesResponse("Kullanıcı eklenebilir.");
        }

        public async Task<OperationResponse<Distributor>> UpdateAsync(Distributor distributor, string password)
        {
            if (!String.IsNullOrEmpty(password))
            {
                byte[] passwordHash, passwordSalt;

                HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                distributor.PasswordHash = passwordHash;
                distributor.PasswordSalt = passwordSalt;
            }

            var result = await _distributorService.UpdateAsync(d => d.Id == distributor.Id, distributor);

            return result;
        }
    }
}
