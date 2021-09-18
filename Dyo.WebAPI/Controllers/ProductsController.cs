using AutoMapper;
using Dyo.Business.Abstract;
using Dyo.Core.Utilities.Cloud;
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

namespace Dyo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        //private readonly ICategoryService _categoryService;
        private readonly ICloudRepo _cloudRepo;

        public ProductsController(IProductService productService, IMapper mapper, ICloudRepo cloudRepo)
        {
            _productService = productService;
            _mapper = mapper;
            //_categoryService = categoryService;
            _cloudRepo = cloudRepo;
        }

        [HttpGet("{productId}")]

        public async Task<IActionResult> GetAsync([FromRoute] string productId)
        {
            var result = await _productService.GetByFilterAsync(p => p.Id.Equals(new ObjectId(productId)));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var productResult = _mapper.Map<Product, ProductForResultDto>(result.Resource);

            return Ok(productResult);
        }

        [HttpGet]
        [Cache(600)]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productService.GetAllAsync();
            if (!products.Success)
            {
                return BadRequest(products.Message);
            }
            var result = _mapper.Map<List<Product>, List<ProductForResultDto>>(products.Resource);

            return Ok(result);
        }

        [HttpGet("distributor/{distributorId}")]
        //[Cache(600)]
        public async Task<IActionResult> GetAllAsync([FromRoute] string distributorId)
        {
            var products = await _productService.GetAllAsync(p => p.DistributorId == new ObjectId(distributorId));

            if (!products.Success)
            {
                return BadRequest(products.Message);
            }
            var result = _mapper.Map<List<Product>, List<ProductForResultDto>>(products.Resource);

            return Ok(result);
        }


        [HttpPost("{productId}/images")]
        //rollere göre kontrol
        public async Task<IActionResult> AddImageForProduct([FromRoute] string productId, List<IFormFile> files)
        {
            var productResult = await _productService.GetByFilterAsync(p => p.Id == new MongoDB.Bson.ObjectId(productId));

            if (!productResult.Success)
            {
                return BadRequest(productResult.Message);
            }

            if (files.Count > 0)
            {
                foreach (var image in files)
                {

                    if (image.Length > 0)
                    {
                        using (var stream = image.OpenReadStream())
                        {
                            var uploadResult = await _cloudRepo.UploadAsync(image.Name, stream);

                            productResult.Resource.Images.Add(new ProductImage
                            {
                                PublicId = uploadResult.PublicId,
                                Url = uploadResult.Url
                            });
                        }
                    }
                }
            }
            var updateResult = await _productService.UpdateAsync(p => p.Id == productResult.Resource.Id, productResult.Resource);

            if (!updateResult.Success)
            {
                return BadRequest(updateResult.Message);
            }
            var result = _mapper.Map<Product, ProductForResultDto>(updateResult.Resource);
            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ProductForCreateDto productForCreateDto)
        {
            var product = _mapper.Map<ProductForCreateDto, Product>(productForCreateDto);

            var added = await _productService.AddAsync(product);

            if (!added.Success)
            {
                return BadRequest(added.Message);
            }
            var result = _mapper.Map<Product, ProductForResultDto>(added.Resource);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] ProductForResultDto productDto)
        {
            var product = _mapper.Map<ProductForResultDto, Product>(productDto);
            var updated = await _productService.UpdateAsync(p => p.Id == product.Id, product);

            if (!updated.Success)
            {
                return BadRequest(updated.Message);
            }

            var result = _mapper.Map<Product, ProductForResultDto>(updated.Resource);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] string productId)
        {
            var deleted = await _productService.DeleteAsync(new Product { Id = new ObjectId(productId) });

            if (!deleted.Success)
            {
                return BadRequest(deleted.Message);
            }
            var result = _mapper.Map<Product, ProductForResultDto>(deleted.Resource);

            return Ok(result);
        }

        //public async Task<IActionResult> AddProductPhoto(string productId, [FromBody] )

        [HttpPost("test")]
        public async Task<IActionResult> GetByFiltered(SearchByFilter filter)
        {
            var results = await _productService.GetAllAsync();
            return Ok();
        }

        [HttpGet("{id}/{categoryName}/{distributorId}")]
        public async Task<IActionResult> GetProductRelated([FromRoute] string id, [FromRoute] string categoryName, [FromRoute] string distributorId)
        {
            var relateds = await _productService.GetAllAsync(p => p.ProductCategory.CategoryName == categoryName && p.DistributorId == new ObjectId(distributorId) && p.Id != new ObjectId(id));

            if (!relateds.Success)
            {
                return BadRequest(relateds.Message);
            }
            var relatedResource = relateds.Resource;

            relatedResource = relateds.Resource.Take(3).ToList();

            var result = _mapper.Map<List<Product>, List<ProductForResultDto>>(relatedResource);
            return Ok(relatedResource);
        }

        [HttpPost("favorites")]
        public async Task<IActionResult> GetFavorites(string[] favorites)
        {
            var favoritesList = new List<Product>();

            foreach (var item in favorites)
            {
                var product = await _productService.GetByFilterAsync(p => p.Id == new ObjectId(item));
                if (!product.Success)
                {
                    return BadRequest("Favorileriniz getirilemedi!");
                }
                favoritesList.Add(product.Resource);
            }

            var result = _mapper.Map<List<Product>, List<ProductForResultDto>>(favoritesList);

            return Ok(result);
        }

        [HttpPost("cart-items")]
        public async Task<IActionResult> GetCart(string[] cartItems)
        {
            var cartList = new List<Product>();
            foreach (var item in cartItems)
            {
                var product = await _productService.GetByFilterAsync(p => p.Id == new ObjectId(item));
                if (!product.Success)
                {
                    return BadRequest("Favorileriniz getirilemedi!");
                }
                cartList.Add(product.Resource);
            }
            var result = _mapper.Map<List<Product>, List<ProductForResultDto>>(cartList);

            return Ok(result);
        }

        [HttpPost("filters/{distributorId}")]
        public async Task<IActionResult> GetProductsyByFilters([FromRoute] string distributorId, SearchByFilter searchByFilter)
        {
            var products = await _productService.GetAllAsync(p => p.ProductCategory.CategoryName == searchByFilter.CategoryName && p.DistributorId == new ObjectId(distributorId) && (
                p.Price >= searchByFilter.Min && p.Price <= searchByFilter.Max
            ));

            if (!products.Success)
            {
                return BadRequest(products.Message);
            }

            List<Product> filters = products.Resource;
            foreach (var item in products.Resource)
            {
                if(searchByFilter.Branches.Any(b => b == item.ProductCategory.Branch) && !filters.Any(p=>p.Id == item.Id))
                {
                    filters.Add(item);
                }
                if (searchByFilter.EducationTypes.Any(b => b == (int)item.ProductCategory.TypeOfEducation) && !filters.Any(p => p.Id == item.Id))
                {
                    filters.Add(item);
                }
            }

            var result = _mapper.Map<List<Product>, List<ProductForResultDto>>(filters);


            return Ok(result);
        }
    
        [HttpGet("{distributorId}/{searchText}")]
        public async Task<IActionResult> GetProductsBySearch([FromRoute] string distributorId, [FromRoute] string searchText)
        {
            var products = await _productService.GetAllAsync(p=> p.DistributorId == new MongoDB.Bson.ObjectId(distributorId));

            if (!products.Success)
            {
                return BadRequest(products.Message);
            }

            var filtered = products.Resource.Where(p => p.ProductName.ToLower().StartsWith(searchText.ToLower()));

            var result = _mapper.Map<List<Product>, List<ProductForResultDto>>(filtered.ToList());
            return Ok(result);
        }
    
    }
}
