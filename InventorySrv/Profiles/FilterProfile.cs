using InventorySrv.Dtos;
using Shared.Models;
using AutoMapper;

namespace InventorySrv.Profiles
{
    public class FilterProfile : Profile
    {
        public FilterProfile()
        {
            CreateMap<Filters, InventoryFilterDto>();
        }
    }
}
