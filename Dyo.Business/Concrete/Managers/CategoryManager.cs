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
    //public class CategoryManager : ICategoryService
    //{
    //    private ICategoryDal _categoryDal;
    //    public CategoryManager(ICategoryDal categoryDal)
    //    {
    //        _categoryDal = categoryDal;
    //    }

    //    public async Task<OperationResponse<Category>> AddAsync(Category category)
    //    {
    //        try
    //        {
    //            var added = await _categoryDal.AddAsync(category);
    //            return OperationResponse<Category>.CreateSuccesResponse(added);                   
    //        }
    //        catch (Exception ex)
    //        {
    //            return OperationResponse<Category>.CreateFailure(ex.Message);
    //        }
            
    //    }

    //    public async Task<OperationResponse<Category>> DeleteAsync(Category category)
    //    {
    //        try
    //        {
    //            var deleted = await _categoryDal.DeleteAsync(category);
    //            return OperationResponse<Category>.CreateSuccesResponse(deleted);
    //        }
    //        catch (Exception ex)
    //        {
    //            return OperationResponse<Category>.CreateFailure(ex.Message);
    //        }
    //    }

    //    public async Task DeleteManyAsync(Expression<Func<Category, bool>> filter)
    //    {
    //        await _categoryDal.DeleteManyAsync(filter);
    //    }

    //    public async Task<OperationResponse<List<Category>>> GetAllAsync(Expression<Func<Category, bool>> filter = null)
    //    {
    //        try
    //        {
    //            var categories = await _categoryDal.GetAllAsync(filter);
    //            return OperationResponse<List<Category>>.CreateSuccesResponse(categories);
    //        }
    //        catch (Exception ex)
    //        {

    //            return OperationResponse<List<Category>>.CreateFailure(ex.Message);
    //        }
    //    }

    //    public async Task<OperationResponse<Category>> GetByFilterAsync(Expression<Func<Category, bool>> filter)
    //    {
    //        var result = await _categoryDal.GetAsync(filter);
    //        if(result == null)
    //        {
    //            return OperationResponse<Category>.CreateFailure("Kategori bulunamadı");
    //        }
    //        return OperationResponse<Category>.CreateSuccesResponse(result);
    //    }


    //    public async Task<OperationResponse<Category>> UpdateAsync(Expression<Func<Category, bool>> filter, Category category)
    //    {
    //        try
    //        {
    //            var result = await _categoryDal.UpdateAsync(filter, category);
    //            return OperationResponse<Category>.CreateSuccesResponse(result);
    //        }
    //        catch (Exception ex)
    //        {

    //            return OperationResponse<Category>.CreateFailure(ex.Message);
    //        }

    //    }
    //}
}
