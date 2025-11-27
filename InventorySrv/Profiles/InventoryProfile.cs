
using AutoMapper;
using InventorySrv.Dtos;
using InventorySrv.Models;
using Shared.Models;

namespace InventorySrv.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            //S -> D
            CreateMap<InventoryItem, InventoryReadDto>();

            CreateMap<InventoryCreateDto, InventoryItem>();

            CreateMap<InventoryUpdateDto, InventoryItem>();
        }
    }
}