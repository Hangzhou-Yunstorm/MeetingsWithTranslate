using Masuit.Tools.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace Yunstorm.Meeting.Meeting
{
    public class MeetingRoom : AggregateRoot<Guid>
    {
        private MeetingRoom(string sessionCode)
        {
            SessionCode = sessionCode;
            CreationTime = DateTime.UtcNow;

            _messages = new List<Message>();
        }

        /// <summary>
        /// 会话代码
        /// </summary>
        public string SessionCode { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 消息列表
        /// </summary>
        public IReadOnlyCollection<Message> Messages => _messages;
        private readonly List<Message> _messages;

        /// <summary>
        /// 创建一个会议室
        /// </summary>
        /// <param name="creator">创建者</param>
        /// <returns></returns>
        public static MeetingRoom Create(string creator)
        {
            var code = SnowFlake.GetInstance().GetUniqueShortId(8).ToUpper();

            var room = new MeetingRoom(code);

            var message = new Message(null, "System", $"{creator} 创建了会议", "zh-cn");

            room._messages.Add(message);

            return room;
        }

        public void AppendMessage(Message message)
        {
            _messages.Add(message);
        }
    }
}
