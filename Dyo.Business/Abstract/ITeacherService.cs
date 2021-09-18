using Dyo.Core.Utilities.Communication;
using Dyo.Entity.Concrete;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Abstract
{
    public interface ITeacherService
    {
        Task<OperationResponse<List<Teacher>>> GetAllAsync(Expression<Func<Teacher, bool>> filter = null);
        Task<OperationResponse<Teacher>> GetByFilterAsync(Expression<Func<Teacher, bool>> filter);
        Task<OperationResponse<Teacher>> AddAsync(Teacher teacher);
        Task<OperationResponse<Teacher>> UpdateAsync(Expression<Func<Teacher, bool>> filter, Teacher teacher);
        Task<OperationResponse<Teacher>> DeleteAsync(Teacher teacher);
        Task DeleteManyAsync(Expression<Func<Teacher, bool>> filter);
      
    }
}
