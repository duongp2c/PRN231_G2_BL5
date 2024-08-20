using AutoMapper;
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountRegisterDto>().ReverseMap();
            CreateMap<Account, AccountLoginDto>().ReverseMap();
        }
    }
}
