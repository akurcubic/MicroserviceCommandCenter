using AutoMapper;
using Microsoft.Data.SqlClient;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile{

        //Profile klasa: To je osnovna klasa koju AutoMapper koristi za definisanje pravila mapiranja između različitih objekata. Umesto da definišeš mapiranja na više mesta u kodu, organizuješ ih unutar Profile klase.

        public PlatformsProfile()
        {
            //source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
            CreateMap<Platform, GrpcPlatformModel>().ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id)).
            ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).
            ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
        }
    }
}