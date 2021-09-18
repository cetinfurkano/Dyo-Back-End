using Dyo.Business.Abstract;
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
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        //private readonly IDistributorService _distributorService;

        public OrderManager(IOrderDal orderDal /*IDistributorService distributorService*/)
        {
            _orderDal = orderDal;
            //_distributorService = distributorService;
        }

        public async Task<OperationResponse<Order>> AddAsync(Order order)
        {
            try
            {
                var added = await _orderDal.AddAsync(order);

                //_distributorService.UpdateContract(order.Distributor.Id);
                return OperationResponse<Order>.CreateSuccesResponse(added);
            }
            catch (Exception ex)
            {
                return OperationResponse<Order>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Order>> DeleteAsync(Order order)
        {
            try
            {
                var deleted = await _orderDal.DeleteAsync(order);
                return OperationResponse<Order>.CreateSuccesResponse(deleted);
            }
            catch (Exception ex)
            {
                return OperationResponse<Order>.CreateFailure(ex.Message);
            }
        }

        public async Task DeleteManyAsync(Expression<Func<Order, bool>> filter)
        {
            await _orderDal.DeleteManyAsync(filter);
        }

        public async Task<OperationResponse<List<Order>>> GetAllAsync(Expression<Func<Order, bool>> filter = null)
        {
            try
            {
                var orders = await _orderDal.GetAllAsync(filter);
                if(orders == null)
                {
                    return OperationResponse<List<Order>>.CreateFailure("Herhangi bir siparişe rastlanmadı!");
                }
                return OperationResponse<List<Order>>.CreateSuccesResponse(orders);
            }
            catch (Exception ex)
            {
                return OperationResponse<List<Order>>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Order>> GetByFilterAsync(Expression<Func<Order, bool>> filter)
        {
            var result = await _orderDal.GetAsync(filter);
            if (result == null)
            {
                return OperationResponse<Order>.CreateFailure("Sipariş bulunamadı");
            }
            return OperationResponse<Order>.CreateSuccesResponse(result);
        }

        public async Task<OperationResponse<Order>> UpdateAsync(Expression<Func<Order, bool>> filter, Order order)
        {
            try
            {
                var result = await _orderDal.UpdateAsync(filter, order);
                
                return OperationResponse<Order>.CreateSuccesResponse(result);
            }
            catch (Exception ex)
            {

                return OperationResponse<Order>.CreateFailure(ex.Message);
            }
        }
    }
}
