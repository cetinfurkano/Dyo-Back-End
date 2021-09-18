using Dyo.Core.Utilities.Communication;
using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Abstract
{
    public interface IProductService
    {
        Task<OperationResponse<List<Product>>> GetAllAsync(Expression<Func<Product, bool>> filter = null);
        Task<OperationResponse<Product>> GetByFilterAsync(Expression<Func<Product, bool>> filter);
        Task<OperationResponse<Product>> AddAsync(Product product);
        Task<OperationResponse<Product>> UpdateAsync(Expression<Func<Product, bool>> filter,Product product);
        Task<OperationResponse<Product>> DeleteAsync(Product product);
        Task DeleteManyAsync(Expression<Func<Product, bool>> filter);

    }
}
