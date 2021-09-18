using Autofac;
using Autofac.Extras.DynamicProxy;
using Dyo.Business.Abstract;
using Dyo.Business.Concrete.Managers;
using Dyo.DataAccess.Abstract;
using Dyo.DataAccess.Concrete.MongoDB;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;

using System.Text;
using Dyo.Core.Utilities.Security.JWT;

namespace Dyo.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            builder.RegisterType<MdProductDal>().As<IProductDal>().SingleInstance();

            builder.RegisterType<TeacherAuthManager>().As<ITeacherAuthService>().SingleInstance();
            builder.RegisterType<TeacherManager>().As<ITeacherService>().SingleInstance();
            builder.RegisterType<MdTeacherDal>().As<ITeacherDal>().SingleInstance();

            builder.RegisterType<JwtHelper>().As<ITokenHelper>().SingleInstance();

            builder.RegisterType<DistributorManager>().As<IDistributorService>().SingleInstance();
            builder.RegisterType<DistributorAuthManager>().As<IDistributorAuthService>().SingleInstance();
            builder.RegisterType<MdDistributorDal>().As<IDistributorDal>().SingleInstance();

            //builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            //builder.RegisterType<MdCategoryDal>().As<ICategoryDal>().SingleInstance();
           
            builder.RegisterType<OrderManager>().As<IOrderService>().SingleInstance();
            builder.RegisterType<MdOrderDal>().As<IOrderDal>().SingleInstance();




        }
    }
}
