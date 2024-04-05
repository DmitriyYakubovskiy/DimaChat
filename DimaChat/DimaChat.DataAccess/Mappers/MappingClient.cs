using AutoMapper;
using DimaChat.DataAccess.Entities;
using DimaChat.DataAccess.Models;

namespace DimaChat.DataAccess.Mappers;

public class MappingClient : Profile
{
    public MappingClient()
    {
        CreateMap<ClientModel, ClientEntity>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ClientPassword, opt => opt.MapFrom(src => src.Password))
            .ReverseMap();         
    }
}
