using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles: Profile // we derive from Profile to get some cool features offed
    {
        public AutoMapperProfiles()
        {   // we want to map AppUser => MemberDto
            CreateMap<AppUser, MemberDto>();
            
            // we want to map Photo => PhotoDto
            CreateMap<Photo, PhotoDto>();
        }
    }
}