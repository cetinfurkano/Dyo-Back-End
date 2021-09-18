using Dyo.Core.DataAccess.MongoDB;
using Dyo.DataAccess.Abstract;
using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.DataAccess.Concrete.MongoDB
{
    public class MdOrderDal: MdEntityRepositoryBase<Order>, IOrderDal
    {
        public MdOrderDal(IMongoDBSettings settings):base(settings)
        {

        }
    }
}
