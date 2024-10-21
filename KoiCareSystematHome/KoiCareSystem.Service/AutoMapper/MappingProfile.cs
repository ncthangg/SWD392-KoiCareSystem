using AutoMapper;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Common.DTOs.Response;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service.Base;
namespace KoiCareSystem.Service.AutoMapper
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

            CreateMap<ServiceResult, ResponseUserDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Data != null ? ((User)src.Data).Id : default(int?)))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Data != null ? ((User)src.Data).Email : null))
                    .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.Data != null ? ((User)src.Data).IsVerified : default(bool)))
                    .ForMember(dest => dest.EmailVerificationToken, opt => opt.MapFrom(src => src.Data != null ? ((User)src.Data).EmailVerificationToken : null))
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Data != null ? ((User)src.Data).RoleId : default(int?)))
                    .ReverseMap();
        }
    }
}
