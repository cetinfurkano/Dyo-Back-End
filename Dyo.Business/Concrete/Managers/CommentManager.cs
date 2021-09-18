using Dyo.Business.Abstract;
using Dyo.Core.Utilities.Communication;
using Dyo.DataAccess.Abstract;
using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dyo.Business.Concrete.Managers
{
    public class CommentManager : ICommentService
    {
       private ICommentDal _commentDal;
        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public async Task<OperationResponse<Comment>> AddAsync(Comment comment)
        {
            try
            {
                var added = await _commentDal.AddAsync(comment);
                return OperationResponse<Comment>.CreateSuccesResponse(added);
            }
            catch (Exception ex)
            {
                return OperationResponse<Comment>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Comment>> DeleteAsync(Comment comment)
        {
            try
            {
                var deleted = await _commentDal.DeleteAsync(comment);
                return OperationResponse<Comment>.CreateSuccesResponse(deleted);
            }
            catch (Exception ex)
            {
                return OperationResponse<Comment>.CreateFailure(ex.Message);
            }
        }

        public async Task DeleteManyAsync(Expression<Func<Comment, bool>> filter)
        {
            await _commentDal.DeleteManyAsync(filter);
        }

        public async Task<OperationResponse<List<Comment>>> GetAllAsync(Expression<Func<Comment, bool>> filter = null)
        {
            try
            {
                var comments = await _commentDal.GetAllAsync(filter);
                return OperationResponse<List<Comment>>.CreateSuccesResponse(comments);
            }
            catch (Exception ex)
            {

                return OperationResponse<List<Comment>>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Comment>> GetByFilterAsync(Expression<Func<Comment, bool>> filter)
        {
            var result = await _commentDal.GetAsync(filter);
            if (result == null)
            {
                return OperationResponse<Comment>.CreateFailure("Yorum bulunamadı");
            }
            return OperationResponse<Comment>.CreateSuccesResponse(result);
        }

       

        public async Task<OperationResponse<Comment>> UpdateAsync(Expression<Func<Comment, bool>> filter, Comment comment)
        {
            try
            {
                var result = await _commentDal.UpdateAsync(filter, comment);
                return OperationResponse<Comment>.CreateSuccesResponse(result);
            }
            catch (Exception ex)
            {

                return OperationResponse<Comment>.CreateFailure(ex.Message);
            }
        }
    }
}
