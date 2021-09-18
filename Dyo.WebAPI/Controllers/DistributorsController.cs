using AutoMapper;
using Dyo.Business.Abstract;
using Dyo.Business.ValidationRules.FluentValidation.DtoValidation;
using Dyo.Core.Utilities.Cloud;
using Dyo.Core.Utilities.Security.JWT;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using Dyo.WebAPI.Attributes;
using Dyo.WebAPI.HelperDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dyo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistributorsController : Controller
    {
        private readonly IDistributorAuthService _distributorAuthService;
        private readonly IDistributorService _distributorService;
        private readonly ITeacherService _teacherService;
        private readonly ITeacherAuthService _teacherAuthService;
        private readonly ICloudRepo _cloudRepo;
        private readonly IMapper _mapper;

        public DistributorsController(IDistributorAuthService distributorAuthService, IMapper mapper, IDistributorService distributorService, ITeacherService teacherService, ICloudRepo cloudRepo, ITeacherAuthService teacherAuthService)
        {
            _distributorAuthService = distributorAuthService;
            _distributorService = distributorService;
            _teacherService = teacherService;
            _teacherAuthService = teacherAuthService;
            _cloudRepo = cloudRepo;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [Validate(typeof(DistributorForLoginValidator))]
        public async Task<IActionResult> Login([FromBody] DistributorForLoginDto distributorForLogin)
        {
            var distributorLoginResult = await _distributorAuthService.LoginAsync(distributorForLogin);
            if (!distributorLoginResult.Success)
            {
                return BadRequest(distributorLoginResult.Message);
            }
            var result = _distributorAuthService.CreateAccessToken(distributorLoginResult.Resource);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Resource);
        }

        [HttpPost("register")]
        [Validate(typeof(DistributorForRegisterValidator))]
        public async Task<IActionResult> Register([FromBody] DistributorForRegisterDto distributorForRegisterDto)
        {
            var userExists = await _distributorAuthService.UserExistsAsync(distributorForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }
            var teacher = _mapper.Map<DistributorForRegisterDto, Distributor>(distributorForRegisterDto);

            var registerResult = await _distributorAuthService.RegisterAsync(teacher, distributorForRegisterDto.Password);

            if (registerResult.Success)
            {
                var result = _distributorAuthService.CreateAccessToken(registerResult.Resource);

                if (result.Success)
                {
                    return Ok(result.Resource);
                }

                return BadRequest(result.Message);
            }
            return BadRequest(registerResult.Message);
        }

        [HttpPut]
        [Validate(typeof(DistributorForUpdateValidator))]
        public async Task<IActionResult> PutAsync([FromBody] DistributorForUpdateDto distributorForUpdate)
        {
            var distributor = _mapper.Map<DistributorForUpdateDto, Distributor>(distributorForUpdate);
            var distributorWillUpdate = await _distributorService.GetByFilterAsync(d => d.Id == distributor.Id);
            if (!distributorWillUpdate.Success)
            {
                return BadRequest("Bir şeyler ters gitti");
            }
            distributorWillUpdate.Resource.FirstName = distributor.FirstName;
            distributorWillUpdate.Resource.LastName = distributor.LastName;
            distributorWillUpdate.Resource.OfficeAddress = distributor.OfficeAddress;
            distributorWillUpdate.Resource.PhoneNumber = distributor.PhoneNumber;

            var updatedResult = await _distributorAuthService.UpdateAsync(distributorWillUpdate.Resource, distributorForUpdate.Password);
            if (!updatedResult.Success)
            {
                return BadRequest(updatedResult.Message);
            }

            var result = _mapper.Map<Distributor, DistributorForGetResultDto>(updatedResult.Resource);
            return Ok(result);
    }

        [HttpGet("{distributorId}")]
        public async Task<IActionResult> GetAsync([FromRoute] string distributorId)
        {
            var distributor = await _distributorService.GetByFilterAsync(d => d.Id == new ObjectId(distributorId));
            if (!distributor.Success)
            {
                return BadRequest(distributor.Message);
            }

            var result = _mapper.Map<Distributor, DistributorForGetResultDto>(distributor.Resource);

            return Ok(result);
        }

        [HttpGet("{distributorId}/contracts/{publisherName}/details")]
        [Cache(600)]
        public async Task<IActionResult> GetContractDetails(string distributorId, string publisherName)
        {
            var result = await _distributorService.GetContractDetails(new ObjectId(distributorId), publisherName);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Resource);
        }

        [HttpGet("{distributorId}/contracts")]
        //[Cache(600)]
        public async Task<IActionResult> GetContracts(string distributorId)
        {
            var result = await _distributorService.GetContracts(new ObjectId(distributorId));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Resource);
        }
       

        [HttpPost("{distributorId}/contracts")]
        public async Task<IActionResult> AddContract([FromRoute]string distributorId, [FromBody] ContractForCreateDto contractDto)
        {
            var contract = _mapper.Map<ContractForCreateDto, Contract>(contractDto);
            
            var added = await _distributorService.AddContract(new ObjectId(distributorId), contract);

            if (!added.Success)
            {
                return BadRequest(added.Message);
            }
            return Ok(added.Resource);
        }

        [HttpPut("{distributorId}/contracts/{publisherName}")]
        public async Task<IActionResult> UpdateContract([FromRoute] string distributorId, string publisherName, [FromBody] Contract contract)
        {
            
            var added = await _distributorService.UpdateContract(new ObjectId(distributorId), contract, publisherName);

            if (!added.Success)
            {
                return BadRequest(added.Message);
            }
            return Ok(added.Resource);
        }

        [HttpDelete("{distributorId}/contracts")]
        public async Task<IActionResult> RemoveContract([FromRoute] string distributorId, [FromBody] Contract contract)
        {
            var added = await _distributorService.RemoveContract(new ObjectId(distributorId), contract);

            if (!added.Success)
            {
                return BadRequest(added.Message);
            }
            return Ok(added.Resource);
        }
        
        [HttpGet("{distributorId}/teachers")]
        public async Task<IActionResult> GetDistributorTeachers([FromRoute] string distributorId)
        {
            var distTeachersResult = await _teacherService.GetAllAsync(t=> t.Distributors.Contains(new ObjectId(distributorId)));
            if (!distTeachersResult.Success)
            {
                return BadRequest(distTeachersResult.Message);
            }
            var result = _mapper.Map<List<Teacher>, List<TeacherForGetResult>>(distTeachersResult.Resource);
            return Ok(result);
        }
        [HttpDelete("{distributorId}/teachers/{teacherId}")]
        public async Task<IActionResult> RemoveTeacher([FromRoute] string distributorId, [FromRoute] string teacherId){
            var teacher = await _teacherService.GetByFilterAsync(t => t.Id == new ObjectId(teacherId));
            if (!teacher.Success)
            {
                return BadRequest(teacher.Message);
            }
           var dist =  teacher.Resource.Distributors.FirstOrDefault(d => d == new ObjectId(distributorId));
            teacher.Resource.Distributors.Remove(dist);
           var updated = await _teacherService.UpdateAsync(t => t.Id == teacher.Resource.Id, teacher.Resource);
            if (!updated.Success)
            {
                return BadRequest(updated.Message);
            }

            var result = _mapper.Map<Teacher, TeacherForGetResult>(updated.Resource);
            return Ok(result);

        }

        [HttpGet("{distributorId}/statistics")]
        public async Task<IActionResult> GetDistributorStatistics(string distributorId)
        {
            var distributorGet = await _distributorService.GetByFilterAsync(d => d.Id == new ObjectId(distributorId));
            if (!distributorGet.Success)
            {
                return BadRequest(distributorGet.Message);
            }
            var distributorTeachers = await _teacherService.GetAllAsync(t => t.Distributors.Contains(distributorGet.Resource.Id));
            if (!distributorTeachers.Success)
            {
                return BadRequest(distributorTeachers.Message);

            }
            var statistics = await _distributorService.GetDistributorStatisticss(distributorGet.Resource.Id);
            if (!statistics.Success)
            {
                return BadRequest(statistics.Message);
            }
            statistics.Resource.LinkedTeacherCount = distributorTeachers.Resource.Count;
            return Ok(statistics.Resource);
        }

        [HttpGet("{distributorId}/statistics/monthly")]
        public async Task<IActionResult> GetDistributorMontlyStatistics(string distributorId)
        {
            var distributorStatistics = await _distributorService.GetDistributorMontlyStatistics(new ObjectId(distributorId));
            if (!distributorStatistics.Success)
            {
                return BadRequest(distributorStatistics.Message);
            }
            return Ok(distributorStatistics.Resource);
        }

        [HttpPut("{distributorId}/profilePhoto")] 
        public async Task<IActionResult> AddProfilePhoto(string distributorId, IFormFile file)
        {
            var distributorResult = await _distributorService.GetByFilterAsync(d => d.Id == new MongoDB.Bson.ObjectId(distributorId));

            if (!distributorResult.Success)
            {
                return BadRequest(distributorResult.Message);
            }


            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadResult = await _cloudRepo.UploadAsync(file.Name, stream);

                    distributorResult.Resource.ProfilePhoto = new ProfilePhoto
                    {
                        PublicId = uploadResult.PublicId,
                        Url = uploadResult.Url
                    };

                    var updateResult = await _distributorService.UpdateAsync(p => p.Id == distributorResult.Resource.Id, distributorResult.Resource);

                    if (!updateResult.Success)
                    {
                        return BadRequest(updateResult.Message);
                    }

                    var result = _mapper.Map<Distributor, DistributorForGetResultDto>(updateResult.Resource);
                    return Ok(result.ProfilePhoto);
                }
            }

            return BadRequest("Bir hata meydana geldi daha sonra tekrar deneyin!");
        }

        [HttpPost("teachers")]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherForRegisterDto teacherWillAdd)
        {
            var teacher = _mapper.Map<TeacherForRegisterDto, Teacher>(teacherWillAdd);
            var registerResult = await _teacherAuthService.RegisterAsync(teacher, teacherWillAdd.Password);

            if (!registerResult.Success)
            {
                return BadRequest(registerResult.Message);
            }
            var result = _mapper.Map<Teacher, TeacherForGetResult>(registerResult.Resource);
            return Ok(result);
        }
    }
}
