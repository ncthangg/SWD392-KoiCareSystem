using AutoMapper;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Ánh xạ từ Order sang OrderDTO và ngược lại
            //CreateMap<Order, OrderDTO>();
            //CreateMap<OrderDTO, Order>();

        }
    }
}
