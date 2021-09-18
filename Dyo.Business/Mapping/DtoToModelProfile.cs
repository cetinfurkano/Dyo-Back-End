using AutoMapper;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Business.Mapping
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<DistributorForRegisterDto, Distributor>();
            CreateMap<DistributorForUpdateDto, Distributor>().ForMember(src=> src.Id, opt=> opt.MapFrom(src=> new MongoDB.Bson.ObjectId(src.Id)));
            CreateMap<DistributorForTeacherSelectDto, Distributor>().ForMember(src=> src.Id, opt => opt.MapFrom(src => new MongoDB.Bson.ObjectId(src.Id)));

            CreateMap<TeacherForRegisterDto, Teacher>().ForMember(src=> src.Distributors, opt=> opt.MapFrom(src=> new List<MongoDB.Bson.ObjectId> {new MongoDB.Bson.ObjectId(src.DistributorId) }));
            
            CreateMap<TeacherForGetResult, Teacher>();
            CreateMap<TeacherForUpdateDto, Teacher>().ForMember(src => src.Id, opt => opt.MapFrom(src => new MongoDB.Bson.ObjectId(src.Id)));
            CreateMap<TeacherForOrderResult, Teacher>();

            CreateMap<CategoryForRegisterDto, Category>();

            CreateMap<ContractForCreateDto, Contract>();

            CreateMap<ProductForCreateDto, Product>().ForMember(src=> src.DistributorId, opt => opt.MapFrom(src=> new MongoDB.Bson.ObjectId(src.DistributorId)));

            CreateMap<ProductForResultDto, Product>().ForMember(src=> src.DistributorId, opt => opt.MapFrom(src=> new MongoDB.Bson.ObjectId(src.DistributorId))).ForMember(src=>src.Id, opt => opt.MapFrom(src=> new MongoDB.Bson.ObjectId(src.Id)));

            CreateMap<CategoryDto, Category>().ForMember(src=> src.TypeOfEducation, opt => opt.MapFrom(src=> (ETypesOfEducation)src.TypeOfEducation));

            CreateMap<OrderItemForCreateDto, OrderItem>().ForMember(src => src.Product, opt => opt.MapFrom(src=> new Product { Id = new MongoDB.Bson.ObjectId(src.ProductId) } ));

            CreateMap<OrderForCreateDto, Order>().ForMember(src => src.OrderState, opt => opt.MapFrom(src => (EOrderState)src.OrderState)).ForMember(src => src.Distributor, opt => opt.MapFrom(src => new Distributor
            {
                Id = new MongoDB.Bson.ObjectId(src.DistributorId)
            })).ForMember(src => src.Teacher, opt => opt.MapFrom(src => new Teacher
            {
                Id = new MongoDB.Bson.ObjectId(src.TeacherId)
            }));

            CreateMap<OrderForUpdateDto, Order>().ForMember(src => src.OrderState, opt => opt.MapFrom(src => (EOrderState)src.OrderState)).ForMember(src => src.Distributor, opt => opt.MapFrom(src => new Distributor
            {
                Id = new MongoDB.Bson.ObjectId(src.DistributorId)
            })).ForMember(src => src.Teacher, opt => opt.MapFrom(src => new Teacher
            {
                Id = new MongoDB.Bson.ObjectId(src.TeacherId)
            })).ForMember(src => src.Id, opt => opt.MapFrom(src => new MongoDB.Bson.ObjectId(src.OrderId)));

            CreateMap<ContractForCreateDto, Contract>();



        }
    }
}
