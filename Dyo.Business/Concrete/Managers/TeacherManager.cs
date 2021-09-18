using Dyo.Business.Abstract;
using Dyo.Core.Utilities.Communication;
using Dyo.DataAccess.Abstract;
using Dyo.Entity.Concrete;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Concrete.Managers
{
    public class TeacherManager : ITeacherService
    {
        private ITeacherDal _teacherDal;
        private readonly IDistributorService _distributorService;
        public TeacherManager(ITeacherDal teacherDal, IDistributorService distributorService)
        {
            _teacherDal = teacherDal;
            _distributorService = distributorService;
        }

        public async Task<OperationResponse<Teacher>> AddAsync(Teacher teacher)
        {
            var check = await _teacherDal.GetAsync(t => t.Email == teacher.Email);
            if(check != null && !teacher.Email.Equals("defaultTeacher@email.com"))
            {
                return OperationResponse<Teacher>.CreateFailure("Böyle bir kullanıcı zaten var!");
            }
            if (!teacher.Distributors.Any())
            {
                return OperationResponse<Teacher>.CreateFailure("Öğretmen kayıt olurken bir distribütör referansı vermelidir.");
            }
            var distributorCheck = await _distributorService.GetByFilterAsync(d => d.Id == teacher.Distributors[0]);
            if (!distributorCheck.Success)
            {
                return OperationResponse<Teacher>.CreateFailure(distributorCheck.Message);
            }

            try
            {
                teacher.Roles.Add("teacher");
                var added = await _teacherDal.AddAsync(teacher);
                return OperationResponse<Teacher>.CreateSuccesResponse(added);
            }
            catch (Exception ex)
            {
                return OperationResponse<Teacher>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Teacher>> DeleteAsync(Teacher teacher)
        {
            try
            {
                var deleted = await _teacherDal.DeleteAsync(teacher);
                return OperationResponse<Teacher>.CreateSuccesResponse(deleted);
            }
            catch (Exception ex)
            {
                return OperationResponse<Teacher>.CreateFailure(ex.Message);
            }
        }

        public async Task DeleteManyAsync(Expression<Func<Teacher, bool>> filter)
        {
            await _teacherDal.DeleteManyAsync(filter);
        }

        public async Task<OperationResponse<List<Teacher>>> GetAllAsync(Expression<Func<Teacher, bool>> filter = null)
        {
            try
            {
                var teachers = await _teacherDal.GetAllAsync(filter);
                return OperationResponse<List<Teacher>>.CreateSuccesResponse(teachers);
            }
            catch (Exception ex)
            {

                return OperationResponse<List<Teacher>>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Teacher>> GetByFilterAsync(Expression<Func<Teacher, bool>> filter)
        {
            var result = await _teacherDal.GetAsync(filter);
            if (result == null)
            {
                return OperationResponse<Teacher>.CreateFailure("Öğretmen bulunamadı");
            }
            return OperationResponse<Teacher>.CreateSuccesResponse(result);
        }

      

        public async Task<OperationResponse<Teacher>> UpdateAsync(Expression<Func<Teacher, bool>> filter, Teacher teacher)
        {
            try
            {
                var result = await _teacherDal.UpdateAsync(filter, teacher);
                return OperationResponse<Teacher>.CreateSuccesResponse(result);
            }
            catch (Exception ex)
            {

                return OperationResponse<Teacher>.CreateFailure(ex.Message);
            }
        }

    }
}
