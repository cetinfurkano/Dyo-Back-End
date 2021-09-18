using Dyo.Core.DataAccess.MongoDB;
using Dyo.DataAccess.Abstract;
using Dyo.Entity.Concrete;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.DataAccess.Concrete.MongoDB
{
    public class MdDistributorDal: MdEntityRepositoryBase<Distributor>,IDistributorDal
    {
        public MdDistributorDal(IMongoDBSettings settings) : base(settings)
        {

        }
      
    }
}
