
using AutoMapper;
using InventorySrv.Dtos;
using InventorySrv.Models;

namespace InventorySrv.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //S -> D
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
        }
    }
}