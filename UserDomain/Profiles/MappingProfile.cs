using UserDomain.Models.Entities;
using UserDomain.Models.Response;
using AutoMapper;

namespace UserDomain.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDomain.Models.Entities.User, UserResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.LastLoginAt, opt => opt.MapFrom(src => src.LastLoginAt));
    }
}