using Dyo.Core.DataAccess.MongoDB;
using Dyo.DataAccess.Abstract;
using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.DataAccess.Concrete.MongoDB
{
    public class MdProductDal:MdEntityRepositoryBase<Product>, IProductDal
    {
        public MdProductDal(IMongoDBSettings settings) : base(settings)
        {

        }
    }
}
