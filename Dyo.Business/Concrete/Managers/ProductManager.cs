using Dyo.Business.Abstract;
using Dyo.Core.DataAccess.MongoDB;
using Dyo.Core.Utilities.Communication;
using Dyo.DataAccess.Abstract;
using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Concrete.Managers
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public async Task<OperationResponse<Product>> AddAsync(Product product)
        {
            try
            {
                var added = await _productDal.AddAsync(product);
                return OperationResponse<Product>.CreateSuccesResponse(added);
            }
            catch (Exception ex)
            {
                return OperationResponse<Product>.CreateFailure(ex.Message);
            }
        }

       
        public async Task<OperationResponse<Product>> DeleteAsync(Product product)
        {
            try
            {
                var deleted = await _productDal.DeleteAsync(product);
                return OperationResponse<Product>.CreateSuccesResponse(deleted);
            }
            catch (Exception ex)
            {
                return OperationResponse<Product>.CreateFailure(ex.Message);
            }
        }

        public async Task DeleteManyAsync(Expression<Func<Product, bool>> filter)
        {
            await _productDal.DeleteManyAsync(filter);
        }

        public async Task<OperationResponse<List<Product>>> GetAllAsync(Expression<Func<Product, bool>> filter = null)
        {
            try
            {
                var products = await _productDal.GetAllAsync(filter);
                return OperationResponse<List<Product>>.CreateSuccesResponse(products);
            }
            catch (Exception ex)
            {

                return OperationResponse<List<Product>>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Product>> GetByFilterAsync(Expression<Func<Product, bool>> filter)
        {
            var result = await _productDal.GetAsync(filter);
            if (result == null)
            {
                return OperationResponse<Product>.CreateFailure("Ürün bulunamadı");
            }
            return OperationResponse<Product>.CreateSuccesResponse(result);
        }

       

        public async Task<OperationResponse<Product>> UpdateAsync(Expression<Func<Product, bool>> filter, Product product)
        {
            try
            {
                var result = await _productDal.UpdateAsync(filter, product);
                return OperationResponse<Product>.CreateSuccesResponse(result);
            }
            catch (Exception ex)
            {

                return OperationResponse<Product>.CreateFailure(ex.Message);
            }
        }
    }
}
