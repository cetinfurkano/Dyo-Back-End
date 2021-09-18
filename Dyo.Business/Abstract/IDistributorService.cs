using Dyo.Business.Helpers;
using Dyo.Core.Utilities.Communication;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Business.Abstract
{
    public interface IDistributorService
    {
        Task<OperationResponse<List<Distributor>>> GetAllAsync(Expression<Func<Distributor, bool>> filter = null);
        Task<OperationResponse<Distributor>> GetByFilterAsync(Expression<Func<Distributor, bool>> filter);
        Task<OperationResponse<Distributor>> AddAsync(Distributor distributor);
        Task<OperationResponse<Distributor>> UpdateAsync(Expression<Func<Distributor, bool>> filter, Distributor distributor);
        Task<OperationResponse<Distributor>> DeleteAsync(Distributor distributor);
        Task DeleteManyAsync(Expression<Func<Distributor, bool>> filter);
        Task<OperationResponse<Contract>> GetContractDetails(ObjectId distributorId, string publisherName);
        Task<OperationResponse<List<Contract>>> GetContracts(ObjectId distributorId);
        Task<OperationResponse<Contract>> AddContract(ObjectId distributorId, Contract contract);
        Task<OperationResponse<Contract>> RemoveContract(ObjectId distributorId, Contract contract);
        Task<OperationResponse<Contract>> UpdateContract(ObjectId distributorId, Contract contract,string publisherName);
        Task<OperationResponse<DistributorStatistics>> GetDistributorStatisticss(ObjectId distributorId);
        Task<OperationResponse<List<DistributorMontlyOrderStatistics>>> GetDistributorMontlyStatistics(ObjectId distributorId);
        
    }
}
