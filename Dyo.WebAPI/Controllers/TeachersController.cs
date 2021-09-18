using AutoMapper;
using Dyo.Business.Abstract;
using Dyo.Business.ValidationRules.FluentValidation.DtoValidation;
using Dyo.Core.Utilities.Cloud;
using Dyo.Core.Utilities.Security.JWT;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using Dyo.WebAPI.Attributes;
using Dyo.WebAPI.HelperDtos;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dyo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherAuthService _teacherAuthService;
        private readonly ITeacherService _teacherService;
        private readonly IDistributorService _distributorService;
        private readonly ICloudRepo _cloudRepo;
        private readonly IMapper _mapper;

        public TeachersController(ITeacherAuthService teacherAuthService, IMapper mapper, ITeacherService teacherService, IDistributorService distributorService, ICloudRepo cloudRepo)
        {
            _teacherAuthService = teacherAuthService;
            _teacherService = teacherService;
            _distributorService = distributorService;
            _cloudRepo = cloudRepo;
            _mapper = mapper;
        }


        [HttpPost("login")]
        [Validate(typeof(TeacherForLoginValidator))]
        public async Task<IActionResult> Login([FromBody] TeacherForLoginDto teacherForLogin)
        {
            var teacherLoginResult = await _teacherAuthService.LoginAsync(teacherForLogin);
            if (!teacherLoginResult.Success)
            {
                return BadRequest(teacherLoginResult.Message);
            }
            var result = _teacherAuthService.CreateAccessToken(teacherLoginResult.Resource);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Resource);

        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AccessToken), 200)]
        [Validate(typeof(TeacherForRegisterValidator))]
        public async Task<IActionResult> Register([FromBody] TeacherForRegisterDto teacherForRegisterDto)
        {
            var userExists = await _teacherAuthService.UserExistsAsync(teacherForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }
            var teacher = _mapper.Map<TeacherForRegisterDto, Teacher>(teacherForRegisterDto);

            var registerResult = await _teacherAuthService.RegisterAsync(teacher, teacherForRegisterDto.Password);

            if (registerResult.Success)
            {
                var result = _teacherAuthService.CreateAccessToken(registerResult.Resource);

                if (result.Success)
                {
                    return Ok(result.Resource);
                }

                return BadRequest(result.Message);
            }
            return BadRequest(registerResult.Message);
        }

        //validator ekle
        [HttpPut]
        [Validate(typeof(TeacherForUpdateValidator))]
        public async Task<IActionResult> PutAsync([FromBody] TeacherForUpdateDto teacherForUpdate)
        {
            var teacher = _mapper.Map<TeacherForUpdateDto, Teacher>(teacherForUpdate);
            var teacherWillUpdated = await _teacherService.GetByFilterAsync(t => t.Id == teacher.Id);
            if (!teacherWillUpdated.Success)
            {
                return BadRequest("Bir şeyler ters gitti, daha sonra tekrar deneyin!");
            }
            teacherWillUpdated.Resource.Email = teacher.Email;
            teacherWillUpdated.Resource.FirstName = teacher.FirstName;
            teacherWillUpdated.Resource.LastName = teacher.LastName;
            teacherWillUpdated.Resource.PhoneNumber = teacher.PhoneNumber;
            teacherWillUpdated.Resource.School = teacher.School;
            teacherWillUpdated.Resource.Branch = teacher.Branch;
            teacherWillUpdated.Resource.Address = teacher.Address;

            var updateResult = await _teacherAuthService.UpdateAsync(teacherWillUpdated.Resource, teacherForUpdate.Password);
            if (!updateResult.Success)
            {
                return BadRequest(updateResult.Message);
            }
            var result = _mapper.Map<Teacher, TeacherForGetResult>(updateResult.Resource);

            return Ok(result);
        }

        [HttpDelete("{teacherId}")]
        public async Task<IActionResult> DeleteAsync(string teacherId)
        {
            var deleteResult = await _teacherService.DeleteAsync(new Teacher { Id = new MongoDB.Bson.ObjectId(teacherId) });

            if (!deleteResult.Success)
            {
                return BadRequest(deleteResult.Resource);
            }
            var result = _mapper.Map<Teacher, TeacherForGetResult>(deleteResult.Resource);

            return Ok(result);
        }

        [HttpGet("{teacherId}")]
        public async Task<IActionResult> GetAsync([FromRoute] string teacherId)
        {
            var getResult = await _teacherService.GetByFilterAsync(t => t.Id == new MongoDB.Bson.ObjectId(teacherId));

            if (!getResult.Success)
            {
                return BadRequest(getResult.Message);
            }
            var result = _mapper.Map<Teacher, TeacherForGetResult>(getResult.Resource);
            return Ok(result);

        }

        [HttpPut("{teacherId}/distributors/{distributorId}")]
        public async Task<IActionResult> AddDistributorReferance([FromRoute] string distributorId, [FromRoute] string teacherId)
        {
            var teacher = await _teacherService.GetByFilterAsync(t => t.Id == new MongoDB.Bson.ObjectId(teacherId));
            if (!teacher.Success)
            {
                return BadRequest(teacher.Message);
            }
            var distributor =
                await _distributorService.GetByFilterAsync(
                    d => d.Id == new MongoDB.Bson.ObjectId(distributorId));
            if (!distributor.Success)
            {
                return BadRequest("Böyle bir distribütör bulunamadı");
            }
            teacher.Resource.Distributors.Add(new MongoDB.Bson.ObjectId(distributorId));
            var updated = await _teacherService.UpdateAsync(t => t.Id == teacher.Resource.Id, teacher.Resource);
            if (!updated.Success)
            {
                return BadRequest(updated.Message);
            }

            return Ok(new { Id = distributor.Resource.Id, FirstName = distributor.Resource.FirstName, LastName = distributor.Resource.LastName });
        }

        [HttpGet("{teacherId}/distributors")]
        public async Task<IActionResult> GetSelectableDistributors([FromRoute] string teacherId)
        {
            var teacher = await _teacherService.GetByFilterAsync(t => t.Id == new ObjectId(teacherId));
            if (!teacher.Success)
            {
                return BadRequest(teacher.Message);
            }

            List<Distributor> teacherDistributors = new List<Distributor>();

            foreach (var distId in teacher.Resource.Distributors)
            {
                var dist = await _distributorService.GetByFilterAsync(d => d.Id == distId);
                teacherDistributors.Add(dist.Resource);
            }

            var result = _mapper.Map<List<Distributor>, List<DistributorForTeacherSelectDto>> (teacherDistributors);
            return Ok(result);
        }

        [HttpPut("{teacherId}/password")]
        [Validate(typeof(PasswordValidator))]

        public async Task<IActionResult> ChangePassword([FromRoute] string teacherId, [FromBody] ChangePasswordDto passwordDto)
        {
            var teacher = await _teacherService.GetByFilterAsync(t => t.Id == new ObjectId(teacherId));
            if (!teacher.Success)
            {
                return BadRequest(teacher.Resource);
            }

            var updatePasswordResult = await _teacherAuthService.ChangePasswordAsync(teacher.Resource, passwordDto.OldPassword, passwordDto.NewPassword);

            if (!updatePasswordResult.Success)
            {
                return BadRequest(updatePasswordResult.Message);
            }

            var result = _mapper.Map<Teacher, TeacherForGetResult>(updatePasswordResult.Resource);

            return Ok(result);
        }
    }
}
