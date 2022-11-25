using AutoMapper;
using User.Core.Models;
using User.Infrastructure.Dtos;

namespace User.Application.Map
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<UserEntity, UserDto>().ReverseMap();
        }
    }
}
