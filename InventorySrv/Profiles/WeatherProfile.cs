
using AutoMapper;
using InventorySrv.Dtos;
using InventorySrv.Models;

namespace InventorySrv.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            //S -> D
            CreateMap<Inventory, InventoryReadDto>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.UpdateTime));

            var utcNow = DateTime.UtcNow;
            CreateMap<InventoryCreateDto, Inventory>()
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<InventoryUpdateDto, Inventory>()
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}