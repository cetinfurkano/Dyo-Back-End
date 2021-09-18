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
    public interface ITeacherAuthService
    {
        Task<OperationResponse<Teacher>> RegisterAsync(Teacher teacher, string password);
        Task<OperationResponse<Teacher>> LoginAsync(TeacherForLoginDto teacherForLoginDto);
        OperationResponse<AccessToken> CreateAccessToken(Teacher teacher);
        Task<OperationResponse<Teacher>> UserExistsAsync(string email);
        Task<OperationResponse<Teacher>> UpdateAsync(Teacher teacher, string password);
        Task<OperationResponse<Teacher>> ChangePasswordAsync(Teacher teacher, string oldPassword, string newPassword);
    }
}
