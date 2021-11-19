using System.Linq;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile // we derive from Profile to get some cool features offed
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            //1. we can configure a single property mapping
            .ForMember(
                dest => dest.PhotoUrl, // 1. the first part: what to map
                opt => // 2. the second part is the options
                    {  // 3. we specify the options to have the custom mapping (how to map the photoUrl)
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                    });
            CreateMap<Photo, PhotoDto>();

        }
    }
}