using Dyo.Business.Abstract;
using Dyo.Business.Helpers;
using Dyo.Core.Utilities.Communication;
using Dyo.DataAccess.Abstract;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Concrete.Managers
{
    public class DistributorManager : IDistributorService
    {
        private readonly IDistributorDal _distributorDal;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public DistributorManager(IDistributorDal distributorDal, IOrderService orderService, IProductService productService)
        {
            _distributorDal = distributorDal;
            _productService = productService;
            _orderService = orderService;

        }
        public async Task<OperationResponse<Distributor>> AddAsync(Distributor distributor)
        {
            var check = await _distributorDal.GetAsync(d => d.Email == distributor.Email);
            if (check != null)
            {
                return OperationResponse<Distributor>.CreateFailure("Böyle bir kullanıcı zaten var!");
            }

            try
            {
                distributor.Roles.Add("distributor");
                var added = await _distributorDal.AddAsync(distributor);
                return OperationResponse<Distributor>.CreateSuccesResponse(added);
            }
            catch (Exception ex)
            {
                return OperationResponse<Distributor>.CreateFailure(ex.Message);
            }
        }


        public async Task<OperationResponse<Distributor>> DeleteAsync(Distributor distributor)
        {
            try
            {
                var deleted = await _distributorDal.DeleteAsync(distributor);
                return OperationResponse<Distributor>.CreateSuccesResponse(deleted);
            }
            catch (Exception ex)
            {
                return OperationResponse<Distributor>.CreateFailure(ex.Message);
            }
        }

        public async Task DeleteManyAsync(Expression<Func<Distributor, bool>> filter)
        {
            await _distributorDal.DeleteManyAsync(filter);
        }

        public async Task<OperationResponse<List<Distributor>>> GetAllAsync(Expression<Func<Distributor, bool>> filter = null)
        {
            try
            {
                var distributors = await _distributorDal.GetAllAsync(filter);
                return OperationResponse<List<Distributor>>.CreateSuccesResponse(distributors);
            }
            catch (Exception ex)
            {

                return OperationResponse<List<Distributor>>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Distributor>> GetByFilterAsync(Expression<Func<Distributor, bool>> filter)
        {
            var result = await _distributorDal.GetAsync(filter);
            if (result == null)
            {
                return OperationResponse<Distributor>.CreateFailure("Distribütör bulunamadı");
            }
            return OperationResponse<Distributor>.CreateSuccesResponse(result);
        }
        public async Task<OperationResponse<Distributor>> UpdateAsync(Expression<Func<Distributor, bool>> filter, Distributor distributor)
        {
            try
            {
                var result = await _distributorDal.UpdateAsync(filter, distributor);
                return OperationResponse<Distributor>.CreateSuccesResponse(result);
            }
            catch (Exception ex)
            {

                return OperationResponse<Distributor>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Contract>> GetContractDetails(ObjectId distributorId, string publisherName)
        {
            try
            {
                var distributor = await _distributorDal.GetAsync(dist => dist.Id == distributorId);
                if (distributor == null)
                {
                    return OperationResponse<Contract>.CreateFailure("Distribütör bulunamadı!");
                }
                var contract = distributor.Contracts.Where(contract => contract.PublisherName == publisherName).FirstOrDefault();
                if (contract == null)
                {
                    return OperationResponse<Contract>.CreateFailure("Böyle bir anlaşma bulunamadı!");
                }
                var orders = await _orderService.GetAllAsync(o => o.Distributor.Id == distributor.Id);
                if (!orders.Success)
                {
                    return OperationResponse<Contract>.CreateFailure(orders.Message);
                }

                var totalQuantity = GetTotalQuantity(orders.Resource, publisherName);

                return OperationResponse<Contract>.CreateSuccesResponse(new Contract
                {
                    DealAmount = contract.DealAmount,
                    DueDate = contract.DueDate,
                    MaxDiscount = contract.MaxDiscount,
                    Revenues = totalQuantity,
                    StartDate = contract.StartDate,
                    RequiredAmount = contract.DealAmount > totalQuantity ? contract.DealAmount - totalQuantity : 0,
                    CurrentPercent =
                    contract.DealAmount > totalQuantity ? Convert.ToInt32(contract.DealAmount % totalQuantity * 100) : 100
                });
            }
            catch (Exception ex)
            {

                return OperationResponse<Contract>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<List<Contract>>> GetContracts(ObjectId distributorId)
        {
            try
            {
                var distributor = await _distributorDal.GetAsync(dist => dist.Id == distributorId);
                if (distributor == null)
                {
                    return OperationResponse<List<Contract>>.CreateFailure("Distribütör bulunamadı!");
                }
                var contracts = distributor.Contracts;

                var orders = await _orderService.GetAllAsync(o => o.Distributor.Id == distributor.Id);
                if (!orders.Success)
                {
                    return OperationResponse<List<Contract>>.CreateFailure(orders.Message);
                }

                if (orders.Resource.Count > 0 )
                {
                    foreach (var contract in contracts)
                    {
                        var totalQuantity = GetTotalQuantity(orders.Resource, contract.PublisherName);
                        contract.Revenues = totalQuantity;
                        contract.RequiredAmount = contract.DealAmount > totalQuantity ? contract.DealAmount - totalQuantity : 0;
                        contract.CurrentPercent = contract.DealAmount > totalQuantity && totalQuantity > 0 ? Convert.ToInt32(contract.DealAmount % totalQuantity * 100) : contract.DealAmount <= totalQuantity ? 100 : 0;
                    }
                }

                return OperationResponse<List<Contract>>.CreateSuccesResponse(contracts);
            }
            catch (Exception ex)
            {
                return OperationResponse<List<Contract>>.CreateFailure(ex.Message);
            }
        }

        private double GetTotalQuantity(List<Order> orders, string publisherName)
        {
            double totalQuantity = 0;

            foreach (var order in orders)
            {
                foreach (var orderItem in order.Items)
                {
                    if (orderItem.Product.PublisherName == publisherName && order.IsValid && order.OrderState == EOrderState.Completed)
                    {
                        totalQuantity += orderItem.Product.Price * orderItem.Count;
                    }
                }
            }
            return totalQuantity;
        }

        public async Task<OperationResponse<Contract>> AddContract(ObjectId distributorId, Contract contract)
        {
            try
            {
                var distributor = await _distributorDal.GetAsync(d => d.Id == distributorId);
                if (distributor == null)
                {
                    return OperationResponse<Contract>.CreateFailure("Böyle bir distribütör bulunamadı!");
                }
                if (distributor.Contracts.Where(c => c.PublisherName == contract.PublisherName).Count() > 0)
                {
                    return OperationResponse<Contract>.CreateFailure("Böyle bir anlaşma zaten var!");

                }
                distributor.Contracts.Add(contract);
                var updatedDistributor = await _distributorDal.UpdateAsync(d => d.Id == distributor.Id, distributor);
                return OperationResponse<Contract>.CreateSuccesResponse(contract);
            }
            catch (Exception ex)
            {

                return OperationResponse<Contract>.CreateFailure(ex.Message);
            }

        }

        public async Task<OperationResponse<Contract>> RemoveContract(ObjectId distributorId, Contract contract)
        {
            try
            {
                var distributor = await _distributorDal.GetAsync(d => d.Id == distributorId);
                if (distributor == null)
                {
                    return OperationResponse<Contract>.CreateFailure("Böyle bir distribütör bulunamadı!");
                }
                var index = distributor.Contracts.FindIndex(0, distributor.Contracts.Count, c => c.PublisherName == contract.PublisherName);

                distributor.Contracts.RemoveAt(index);

                var updatedDistributor = await _distributorDal.UpdateAsync(d => d.Id == distributor.Id, distributor);
                return OperationResponse<Contract>.CreateSuccesResponse(contract);
            }
            catch (Exception ex)
            {

                return OperationResponse<Contract>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<Contract>> UpdateContract(ObjectId distributorId, Contract contract,string publisherName)
        {
            try
            {
                var distributor = await _distributorDal.GetAsync(d => d.Id == distributorId);
                if (distributor == null)
                {
                    return OperationResponse<Contract>.CreateFailure("Böyle bir distribütör bulunamadı!");
                }
                //var selectedContract = distributor.Contracts.FirstOrDefault(c => c.PublisherName == publisherName);
                var index = distributor.Contracts.FindIndex(0, distributor.Contracts.Count, c => c.PublisherName == publisherName);

                distributor.Contracts[index] = contract;

                var updatedDistributor = await _distributorDal.UpdateAsync(d => d.Id == distributor.Id, distributor);
                return OperationResponse<Contract>.CreateSuccesResponse(contract);
            }
            catch (Exception ex)
            {

                return OperationResponse<Contract>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResponse<DistributorStatistics>> GetDistributorStatisticss(ObjectId distributorId)
        {
            var distributor = await _distributorDal.GetAsync(d => d.Id == distributorId);
            if (distributor == null)
            {
                return OperationResponse<DistributorStatistics>.CreateFailure("Böyle bir distributor bulunamadı");
            }
            var getProducts = await _productService.GetAllAsync(p => p.DistributorId == distributor.Id);
            if (!getProducts.Success)
            {
                return OperationResponse<DistributorStatistics>.CreateFailure(getProducts.Message);
            }
            var productsCount = getProducts.Resource.Count;
            var getOrders = await _orderService.GetAllAsync(o => o.Distributor.Id == distributor.Id);
            if (!getOrders.Success)
            {
                return OperationResponse<DistributorStatistics>.CreateFailure(getOrders.Message);
            }
            var ordersCount = getOrders.Resource.Count;
            var totalOrdersThisMonth = getOrders.Resource.Where(o => o.DueDate.Month == DateTime.Now.Month);
            double totalGainThisMonth = 0;
            double turnoverThisMonth = 0;
            double totalCostThisMonth = 0;

            foreach (var order in totalOrdersThisMonth)
            {
                foreach (var item in order.Items)
                {
                    totalGainThisMonth += (item.Product.Price - item.Product.Cost) * item.Count;
                    turnoverThisMonth += item.Product.Price * item.Count;
                    totalCostThisMonth += item.Product.Cost * item.Count;
                }
            }
            var publishersCount = distributor.Contracts.Count;
            var istatistics = new DistributorStatistics
            {
                ProductCount = productsCount,
                PublishersCount = publishersCount,
                TotalGainThisMonth = totalGainThisMonth,
                TotalOrderThisMonth = totalOrdersThisMonth.Count(),
                TurnoverThisMonth = turnoverThisMonth,
                TotalOrder = ordersCount,
                TotalCostThisMonth = totalCostThisMonth
            };
            return OperationResponse<DistributorStatistics>.CreateSuccesResponse(istatistics);
        }

        public async Task<OperationResponse<List<DistributorMontlyOrderStatistics>>> GetDistributorMontlyStatistics(ObjectId distributorId)
        {
            List<DistributorMontlyOrderStatistics> statistics = new List<DistributorMontlyOrderStatistics>();
            var thisTime = DateTime.Now;
            var orders = await _orderService.GetAllAsync(o => o.Distributor.Id == distributorId);
            if (!orders.Success)
            {
                return OperationResponse<List<DistributorMontlyOrderStatistics>>.CreateFailure("Aylık istatistikler getirilirken bir hata meydana geldi");
            }
            int counter = 0;
            while (counter<6)
            {
                var orderResource = orders.Resource.Where(o => o.DueDate.Month == thisTime.Month);
                double totalGain = 0;
                double totalTurnover = 0;
                double totalCost = 0;
                foreach (var order in orderResource)
                {
                    foreach (var item in order.Items)
                    {
                        totalCost += item.Product.Cost * item.Count;
                        totalGain += (item.Product.Price - item.Product.Cost) * item.Count;
                        totalTurnover += item.Product.Price * item.Count;
                    }
                }
                statistics.Add(new DistributorMontlyOrderStatistics
                {
                    TotalCost = totalCost,
                    TotalGain = totalGain,
                    TotalTurnover = totalTurnover,
                    Month = thisTime.Month
                });
                thisTime = thisTime.AddMonths(-1);
                counter++;
            }
      


            return OperationResponse<List<DistributorMontlyOrderStatistics>>.CreateSuccesResponse(statistics);

        }

        
    }
}
