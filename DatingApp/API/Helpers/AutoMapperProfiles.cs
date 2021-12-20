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

            CreateMap<MemberUpdateDTO, AppUser>();

            CreateMap<RegisterDto, AppUser>();

            //1. add the mapping:
            CreateMap<Message, MessageDto>()
            .ForMember(
                dest => dest.SenderPhotoUrl,// we mapping to SenderPhotoUrl 
                opt => opt.MapFrom(         // we mapping from the senders photos, the one that is main
                    src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(
                dest => dest.RecipientPhotoUrl,// we mapping to RecipientPhotoUrl 
                opt => opt.MapFrom(            // we mapping from the Recipient photos, the one that is main
                    src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
            //2. we'll be adding another dto for the receiving of a message by the API
            // * create and go to DTOs/CreateMessageDto.cs
            


        }
    }
}