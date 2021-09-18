using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dyo.Business.Abstract;
using Dyo.Core.Utilities.Cloud;
using Dyo.Entity.Concrete;
using Dyo.WebAPI.HelperDtos;
using Dyo.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dyo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/images")]
    public class ProductImagesController:ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICloudRepo _cloudRepo;

        public ProductImagesController(IProductService productService, IMapper mapper, ICloudRepo cloudRepo)
        {
            _productService = productService;
            _mapper = mapper;
            _cloudRepo = cloudRepo;

        }

       

    }
}
