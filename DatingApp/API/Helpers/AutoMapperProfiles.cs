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
            // 1. we use this to map a value to Age
            // lets see if this helps in postman
            // YaY, it does, no query for hash and salt
            // go back to README.md
            .ForMember( 
                dest => dest.Age, 
                opt => 
                    {  
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge());
                    });
            CreateMap<Photo, PhotoDto>();

        }
    }
}