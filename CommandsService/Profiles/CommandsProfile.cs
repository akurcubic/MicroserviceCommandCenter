using AutoMapper;
using CommandService.Dtos;
using CommandsService.DbContextOptions;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles
{

    public class CommandsProfile : Profile{

        public CommandsProfile()
        {
            //source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishedDto,Platform>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcPlatformModel,Platform>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId)).
            ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).
            ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}