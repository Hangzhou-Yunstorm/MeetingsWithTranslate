using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Yunstorm.Meeting.Meeting
{
    public interface IMeetingRoomManager : IDomainService
    {
        Task<MeetingRoom> CreateAsync(string creator);

        Task<MeetingRoom> GetBySessionCodeAsync(string sessiongCode);
        Task UpdateAsync(MeetingRoom room);
        Task<IEnumerable<Message>> GetMessagesAsync(string sessionCode);
    }
}
