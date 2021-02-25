using AutoMapper;
using Yunstorm.Meeting.Meeting;
using Yunstorm.Meeting.MeetingRoom;
using Yunstorm.Meeting.OnlineMeetingRoom;
using Yunstorm.Meeting.Web.Hubs.Models;

namespace Yunstorm.Meeting.Web
{
    public class MeetingWebAutoMapperProfile : Profile
    {
        public MeetingWebAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Web project.

            CreateMap<OnlineUserViewModel, Participant>()
                .ForMember(p => p.Id, opt => opt.MapFrom(o => o.Id))
                .ForMember(p => p.ConnectionId, opt => opt.MapFrom(o => o.ConnectionId))
                .ForMember(p => p.Nickname, opt => opt.MapFrom(o => o.Nickname))
                .ForMember(p => p.Language, opt => opt.MapFrom(o => o.Language));

            CreateMap<UserJoinViewModel, Participant>();

            CreateMap<Message, ReturnMessage>();

        }
    }
}
