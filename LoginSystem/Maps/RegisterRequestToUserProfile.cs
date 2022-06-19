using AutoMapper;
using LoginSystem.Entities;
using LoginSystem.Models.Register;

namespace LoginSystem.Maps
{
    public class RegisterRequestToUserProfile : Profile
    {
        public RegisterRequestToUserProfile()
        {
            CreateMap<LoginSignUpRequest, User>()
                .ForMember(dest => dest.Username, o => o.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, o => o.MapFrom(src => src.Password));
        }


    }
}
