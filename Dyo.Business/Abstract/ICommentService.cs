using Dyo.Core.Utilities.Communication;
using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Abstract
{
    public interface ICommentService
    {
        Task<OperationResponse<List<Comment>>> GetAllAsync(Expression<Func<Comment, bool>> filter = null);
        Task<OperationResponse<Comment>> GetByFilterAsync(Expression<Func<Comment, bool>> filter);
        Task<OperationResponse<Comment>> AddAsync(Comment comment);
        Task<OperationResponse<Comment>> UpdateAsync(Expression<Func<Comment, bool>> filter, Comment comment);
        Task<OperationResponse<Comment>> DeleteAsync(Comment comment);
        Task DeleteManyAsync(Expression<Func<Comment, bool>> filter);
    }
}
