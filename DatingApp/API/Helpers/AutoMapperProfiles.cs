using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile 
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(
                dest => dest.PhotoUrl, 
                opt => 
                    {  
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                    })
            .ForMember( 
                dest => dest.Age, 
                opt => 
                    {  
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge());
                    });
            CreateMap<Photo, PhotoDto>();

            //1. add this:
            CreateMap<MemberUpdateDTO, AppUser>(); // there is an option of .ReverseMap() but we not using it be because we not using MemberDto
            //2. use this mapping in the UsersController.cs, go there

        }
    }
}