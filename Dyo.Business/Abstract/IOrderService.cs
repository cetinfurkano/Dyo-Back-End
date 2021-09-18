using Dyo.Core.Utilities.Communication;
using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Abstract
{
    public interface IOrderService
    {
        Task<OperationResponse<List<Order>>> GetAllAsync(Expression<Func<Order, bool>> filter = null);
        Task<OperationResponse<Order>> GetByFilterAsync(Expression<Func<Order, bool>> filter);
        Task<OperationResponse<Order>> AddAsync(Order order);
        Task<OperationResponse<Order>> UpdateAsync(Expression<Func<Order, bool>> filter, Order teacher);
        Task<OperationResponse<Order>> DeleteAsync(Order order); 
        Task DeleteManyAsync(Expression<Func<Order, bool>> filter);
    }
}
