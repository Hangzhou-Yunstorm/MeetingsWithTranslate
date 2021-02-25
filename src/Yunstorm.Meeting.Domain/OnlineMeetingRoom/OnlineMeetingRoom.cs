using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunstorm.Meeting.OnlineMeetingRoom
{
    public class OnlineMeetingRoom
    {
        public IReadOnlyCollection<Participant> Participants => participants;
        private readonly List<Participant> participants;

        public string SessionCode { get; set; }

        public OnlineMeetingRoom(string sessionCode)
        {
            SessionCode = sessionCode;
            participants = new List<Participant>();
        }

        public void Join(Participant participant)
        {
            if (!participants.Any(p => p.Id == participant.Id))
            {
                participants.Add(participant);
            }
        }

        public void Leave(string connectionId)
        {
            var participant = Participants.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (participant != null)
            {
                participants.Remove(participant);
            }
        }
    }

    public class Participant
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Language { get; set; }
        public string ConnectionId { get; set; }

        public Participant(string id, string connectionId, string nickname, string language)
        {
            Id = id;
            ConnectionId = connectionId;
            Nickname = nickname;
            Language = language;
        }
    }
}
