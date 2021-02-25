using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Yunstorm.Meeting.Meeting;
using Yunstorm.Meeting.OnlineMeetingRoom;

namespace Yunstorm.Meeting.Web.Controllers
{
    [Route("api/meeting")]
    public class MeetingController : AbpController
    {
        private readonly IMeetingRoomManager _meetingRoomManager;
        private readonly IOnlineMeetingRoomManager _onlineMeetingRoomManager;
        public MeetingController(IMeetingRoomManager meetingRoomManager,
            IOnlineMeetingRoomManager onlineMeetingRoomManager)
        {
            _meetingRoomManager = meetingRoomManager;
            _onlineMeetingRoomManager = onlineMeetingRoomManager;
        }

        [HttpGet, Route("messages")]
        public async Task<IEnumerable<Message>> Messages(string sessionCode)
        {
            if (string.IsNullOrEmpty(sessionCode))
            {
                return null;
            }
            var messages = await _meetingRoomManager.GetMessagesAsync(sessionCode);
            return messages?.OrderBy(m => m.SendingTime);
        }

        [HttpGet, Route("participants")]
        public IEnumerable<Participant> Participants(string sessionCode)
        {
            if (!string.IsNullOrEmpty(sessionCode))
            {
                var room = _onlineMeetingRoomManager.GetOne(sessionCode);
                return room?.Participants;
            }

            return null;
        }
    }
}
