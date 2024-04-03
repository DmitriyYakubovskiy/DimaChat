using AutoMapper;
using DimaChat.DataAccess.Entities;
using DimaChat.DataAccess.Models;

namespace DimaChat.DataAccess.Mappers;

public class MappingMessage : Profile
{
    public MappingMessage()
    {
        CreateMap<MessageModel, MessageEntity>()
            .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MessageContent, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.ChatId))
            .ForPath(dest => dest.Chat.ChatName, opt => opt.MapFrom(src => src.Name));
    }
}
