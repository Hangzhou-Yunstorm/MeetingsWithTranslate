using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Yunstorm.Meeting.Meeting
{
    public interface IMeetingRoomRepository: IRepository<MeetingRoom, Guid>
    {
        Task<MeetingRoom> FindBySessionCodeAsync(string sessionCode);
        Task<MeetingRoom> FindBySessionCodeIncludingMessagesAsync(string sessionCode);
        bool Existed(string sessionCode);
    }
}
