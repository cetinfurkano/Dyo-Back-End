using AutoMapper;
using Dyo.Entity.Concrete;
using Dyo.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Business.Mapping
{
    public class ModelToDtoProfile : Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<Distributor, DistributorForRegisterDto>();
            CreateMap<Distributor, DistributorForGetResultDto>().ForMember(src=> src.Id, opt=> opt.MapFrom(src=> src.Id.ToString()));
             CreateMap<Distributor, DistributorForOrderResultDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<Distributor, DistributorForTeacherSelectDto>().ForMember(src => src.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<Teacher, TeacherForRegisterDto>();
            CreateMap<Teacher, TeacherForUpdateDto>();
            CreateMap<Teacher, TeacherForOrderResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<Teacher, TeacherForGetResult>();


            CreateMap<Category, CategoryForRegisterDto>();

            CreateMap<OrderItem, OrderItemForCreateDto>();
            CreateMap<OrderItem, OrderItemForResultDto>();


            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.TypeOfEducation, opt => opt.MapFrom(src => (int)src.TypeOfEducation));

            CreateMap<Contract, ContractForCreateDto>();


            CreateMap<Order, OrderForResultDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())).
                ForMember(src => src.OrderState, opt => opt.MapFrom(src => (int)src.OrderState));


            CreateMap<Product, ProductForResultDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())).ForMember(dest => dest.DistributorId, opt => opt.MapFrom(src => src.DistributorId.ToString()))/*.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src=> src.Category.CategoryName));*/;

        }
    }
}
