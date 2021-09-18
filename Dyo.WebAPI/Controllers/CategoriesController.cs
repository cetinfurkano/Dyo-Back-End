using AutoMapper;
using Dyo.Business.Abstract;
using Dyo.Business.ValidationRules.FluentValidation.DtoValidation;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using Dyo.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dyo.WebAPI.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class CategoriesController : Controller
    //{
    //    private readonly ICategoryService _categoryService;
    //    private readonly IMapper _mapper;

    //    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    //    {
    //        _categoryService = categoryService;
    //        _mapper = mapper;
    //    }


    //    [HttpGet]
    //    public async Task<IActionResult> GetAsync([FromQuery] string id)
    //    {
    //        var getResult = await _categoryService.GetByFilterAsync(c => c.Id == new ObjectId(id));

    //        if (!getResult.Success)
    //        {
    //            return BadRequest(getResult.Message);
    //        }

    //        var result = _mapper.Map<Category, CategoryForResultDto>(getResult.Resource);

    //        return Ok(result);
    //    }

    //    [HttpGet]
    //    [Cache(600)]
    //    public async Task<IActionResult> GetAsync()
    //    {
    //        var getResult = await _categoryService.GetAllAsync();

    //        if (!getResult.Success)
    //        {
    //            return BadRequest(getResult.Message);
    //        }

    //        var result = _mapper.Map<List<Category>, List<CategoryForResultDto>>(getResult.Resource);

    //        return Ok(result);
    //    }


    //    [HttpPost]
    //    [Validate(typeof(CategoryForRegisterValidator))]
    //    [CacheRemove("CategoriesController.Get")]
    //    public async Task<IActionResult> PostAsync([FromBody] CategoryForRegisterDto categoryForRegisterDto)
    //    {
    //        var category = _mapper.Map<CategoryForRegisterDto, Category>(categoryForRegisterDto);
    //        var addResult = await _categoryService.AddAsync(category);

    //        if (!addResult.Success)
    //        {
    //            return BadRequest(addResult.Message);
    //        }

    //        var result = _mapper.Map<Category, CategoryForResultDto>(addResult.Resource);

    //        return Ok(result);
    //    }

    //}
}
