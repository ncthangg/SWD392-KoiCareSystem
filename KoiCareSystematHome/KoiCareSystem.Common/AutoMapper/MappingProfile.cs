using AutoMapper;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Common.DTOs.Response;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // User
            CreateMap<RequestRegisterDto, User>();
            CreateMap<RequestRegisterAdminDto, User>();

            //Order
            CreateMap<Order, RequestCreateOrderDto>();
            CreateMap<RequestCreateOrderDto, Order>();

            CreateMap<Order, ResponseOrderDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<ResponseOrderDto, Order>();
            //Item in Order
            CreateMap<OrderItem, RequestItemToOrderDto>();
            CreateMap<RequestItemToOrderDto, OrderItem>();
            //Product
            CreateMap<Product, RequestCreateANewProductDto>();
            CreateMap<RequestCreateANewProductDto, Product>();
            //WaterParameters
            CreateMap<ImportDataDto, WaterParameter>().ReverseMap();
        }
    }
}
