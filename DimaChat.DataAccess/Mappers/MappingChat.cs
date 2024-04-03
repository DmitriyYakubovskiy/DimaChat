using AutoMapper;
using DimaChat.DataAccess.Entities;
using DimaChat.DataAccess.Models;

namespace DimaChat.DataAccess.Mappers;

public class MappingChat : Profile
{
    public MappingChat()
    {
        CreateMap<ChatModel, ChatEntity>()
            .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ChatName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Clients, opt => opt.MapFrom(src => src.Clients))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));
    }
}
