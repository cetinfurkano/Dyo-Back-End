using AutoMapper;
using Dyo.Business.Abstract;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using Dyo.WebAPI.HelperDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dyo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IDistributorService _distributorService;
        private readonly ITeacherService _teacherService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper, IProductService productService, IDistributorService distributorService, ITeacherService teacherService)
        {
            _orderService = orderService;
            _productService = productService;
            _distributorService = distributorService;
            _teacherService = teacherService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] OrderForCreateDto orderForCreateDto)
        {
            var order = _mapper.Map<OrderForCreateDto, Order>(orderForCreateDto);

            await GetOrderProps(order);

            var added = await _orderService.AddAsync(order);
            if (!added.Success)
            {
                return BadRequest(added.Message);
            }

            

            var result = _mapper.Map<Order, OrderForResultDto>(added.Resource);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateOrderModel updateOrderModel)
        {
            var order = await _orderService.GetByFilterAsync(o => o.Id == new MongoDB.Bson.ObjectId(updateOrderModel.Id));
            if (!order.Success)
            {
                return BadRequest(order.Message);
            }
            order.Resource.IsValid = updateOrderModel.IsValid;
            order.Resource.OrderState = (EOrderState)updateOrderModel.OrderState;
            if(order.Resource.OrderState == EOrderState.Completed)
            {
                order.Resource.DueDate = DateTime.Now;
            }
            var updateResult = await _orderService.UpdateAsync(o => o.Id == order.Resource.Id, order.Resource);

            if (!updateResult.Success)
            {
                return BadRequest(updateResult.Message);
            }

            var result = _mapper.Map<Order, OrderForResultDto>(updateResult.Resource);

            return Ok(result);

        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetAsync([FromRoute] string orderId)
        {
            var order = await _orderService.GetByFilterAsync(o => o.Id == new MongoDB.Bson.ObjectId(orderId));

            if (!order.Success)
            {
                return BadRequest(order.Message);
            }

            var result = _mapper.Map<Order, OrderForResultDto>(order.Resource);

            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var orders = await _orderService.GetAllAsync();
            if (!orders.Success)
            {
                return BadRequest(orders.Message);
            }
            var result = _mapper.Map<List<Order>, List<OrderForResultDto>>(orders.Resource);

            return Ok(result);
        }

        [HttpGet("distributor/{distributorId}")]
        public async Task<IActionResult> GetAllOrders([FromRoute] string distributorId)
        {
            var orders = await _orderService.GetAllAsync(o => o.Distributor.Id == new MongoDB.Bson.ObjectId(distributorId));

            if (!orders.Success)
            {
                return BadRequest(orders.Message);
            }

            var result = _mapper.Map<List<Order>, List<OrderForResultDto>>(orders.Resource);
            return Ok(result);
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetAllOrdersTeacher([FromRoute] string teacherId)
        {
            var orders = await _orderService.GetAllAsync(o => o.Teacher.Id == new MongoDB.Bson.ObjectId(teacherId));

            if (!orders.Success)
            {
                return BadRequest(orders.Message);
            }
            var result = _mapper.Map<List<Order>, List<OrderForResultDto>>(orders.Resource);
           
            return Ok(result);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string orderId)
        {
            var deleted = await _orderService.DeleteAsync(new Order { Id = new MongoDB.Bson.ObjectId(orderId)});

            if (!deleted.Success)
            {
                return BadRequest(deleted.Message);
            }

            var result = _mapper.Map<Order, OrderForResultDto>(deleted.Resource);

            return Ok(result);

        }

        private async Task<Order> GetOrderProps(Order order)
        {
            var teacher = await _teacherService.GetByFilterAsync(t => t.Id == order.Teacher.Id);
            var distributor = await _distributorService.GetByFilterAsync(d => d.Id == order.Distributor.Id);

            foreach (var item in order.Items)
            {
                var productResult = await _productService.GetByFilterAsync(p => p.Id == item.Product.Id);
                item.Product = productResult.Resource;
            }
            order.Distributor = distributor.Resource;
            order.Teacher = teacher.Resource;

            return order;

        }


    }
}
