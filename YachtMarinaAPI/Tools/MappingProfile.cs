using AutoMapper;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Tools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Basket, BasketDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<CreateDocumentDto, Document>();
            CreateMap<Document, DocumentDto>();
            CreateMap<Yacht, YachtDto>();
            CreateMap<CreateYachtDto, Yacht>();
            CreateMap<CreateJourneyDto, Journey>();
            CreateMap<Journey, JourneyDto>();
            CreateMap<UpdateYachtDto, Yacht>();
            CreateMap<CreateInviteDto, Invite>();
            CreateMap<Message, MessageDto>();
            CreateMap<User, UserChatDto>();
            CreateMap<Chat, ChatDto>();
            CreateMap<CreateMessageDto, Message>();
            CreateMap<UserDto, AvatarDto>();
        }
    }
}
