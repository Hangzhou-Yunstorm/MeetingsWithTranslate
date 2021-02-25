using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Yunstorm.Meeting.Meeting
{
    public class MeetingRoomManager : DomainService, IMeetingRoomManager
    {
        private readonly IMeetingRoomRepository _repository;
        public MeetingRoomManager(IMeetingRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<MeetingRoom> CreateAsync(string creator)
        {
            MeetingRoom room;
            var i = 0;
            do
            {
                room = MeetingRoom.Create(creator);
                if (!_repository.Existed(room.SessionCode))
                {
                    break;
                }
                i++;
            } while (i < 5);

            if (i >= 5)
            {
                return null;
            }
            return await _repository.InsertAsync(room);
        }

        public Task<MeetingRoom> GetBySessionCodeAsync(string sessiongCode)
        {
            return _repository.FindBySessionCodeAsync(sessiongCode);
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(string sessionCode)
        {
            var room = await _repository.FindBySessionCodeIncludingMessagesAsync(sessionCode);
            return room?.Messages;
        }

        public Task UpdateAsync(MeetingRoom room)
        {
            return _repository.UpdateAsync(room);
        }
    }
}
