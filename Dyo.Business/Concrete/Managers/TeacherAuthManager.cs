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
    public class TeacherAuthManager : ITeacherAuthService
    {
        private readonly ITeacherService _teacherService;
        private readonly ITokenHelper _tokenHelper;

        public TeacherAuthManager(ITeacherService teacherService, ITokenHelper tokenHelper)
        {
            _teacherService = teacherService;
            _tokenHelper = tokenHelper;
        }

        public OperationResponse<AccessToken> CreateAccessToken(Teacher teacher)
        {
            var roles = teacher.Roles;
            var accessToken = _tokenHelper.CreateToken(teacher, teacher.Email, roles);
            return OperationResponse<AccessToken>.CreateSuccesResponse(accessToken);
        }

        public async Task<OperationResponse<Teacher>> LoginAsync(TeacherForLoginDto teacherForLogin)
        {
            var teachertoCheck = await _teacherService.GetByFilterAsync(t => t.Email == teacherForLogin.Email);
            if (!teachertoCheck.Success)
            {
                return OperationResponse<Teacher>.CreateFailure("Email ya da parola hatalı!"); ;
            }

            if (!HashingHelper.VerifyPasswordHash(teacherForLogin.Password, teachertoCheck.Resource.PasswordHash, teachertoCheck.Resource.PasswordSalt))
            {
                return OperationResponse<Teacher>.CreateFailure("Email ya da parola hatalı!");
            }

            return teachertoCheck;

        }

        public async Task<OperationResponse<Teacher>> RegisterAsync(Teacher teacherForRegister, string password)
        {
            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            teacherForRegister.PasswordHash = passwordHash;
            teacherForRegister.PasswordSalt = passwordSalt;

            var result = await _teacherService.AddAsync(teacherForRegister);

            return result;
        }

        public async Task<OperationResponse<Teacher>> UserExistsAsync(string email)
        {
            var result = await _teacherService.GetByFilterAsync(t => t.Email == email);
            if (result.Success)
            {
                return OperationResponse<Teacher>.CreateFailure("Kullanıcı mevcut");
            }
            return OperationResponse<Teacher>.CreateSuccesResponse("Kullanıcı eklenebilir.");
        }

        public async Task<OperationResponse<Teacher>> UpdateAsync(Teacher teacher, string password)
        {
            if (!String.IsNullOrEmpty(password))
            {
                byte[] passwordHash, passwordSalt;

                HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                teacher.PasswordHash = passwordHash;
                teacher.PasswordSalt = passwordSalt;
            }

            var result = await _teacherService.UpdateAsync(t => t.Id == teacher.Id, teacher);

            return result;
        }

        public async Task<OperationResponse<Teacher>> ChangePasswordAsync(Teacher teacher, string oldPassword, string newPassword)
        {
            if (!HashingHelper.VerifyPasswordHash(oldPassword, teacher.PasswordHash, teacher.PasswordSalt))
            {
                return OperationResponse<Teacher>.CreateFailure("Hatalı giriş yaptınız");
            }

            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
            teacher.PasswordHash = passwordHash;
            teacher.PasswordSalt = passwordSalt;

            var result = await _teacherService.UpdateAsync(t => t.Id == teacher.Id, teacher);

            return result;
        }

    }
}
