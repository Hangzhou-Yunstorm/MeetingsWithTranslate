using Masuit.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunstorm.Meeting.OnlineMeetingRoom
{
    public class OnlineMeetingRoomManager : IOnlineMeetingRoomManager
    {
        protected readonly ConcurrentDictionary<string, OnlineMeetingRoom> Rooms;
        public OnlineMeetingRoomManager()
        {
            Rooms = new ConcurrentDictionary<string, OnlineMeetingRoom>();
        }

        public void AddOrUpdate(string code, OnlineMeetingRoom room)
        {
            Rooms.AddOrUpdate(code, room, (s, o) => room);
        }

        public void Remove(string code)
        {
            Rooms.Remove(code, out var room);
        }

        public OnlineMeetingRoom GetOne(string sessionCode)
        {
            Rooms.TryGetValue(sessionCode, out var room);
            return room;
        }

        public OnlineMeetingRoom GetOrCreate(string code)
        {
            if (!Rooms.TryGetValue(code, out var room))
            {
                room = new OnlineMeetingRoom(code);
                AddOrUpdate(code, room);
            }
            return room;
        }

        public Participant GetParticipantByConnectionId(string code, string connectionId)
        {
            var room = GetOne(code);
            if (room != null)
            {
                return room.Participants?.FirstOrDefault(p => p.ConnectionId == connectionId);
            }
            return null;
        }

        public IEnumerable<OnlineMeetingRoom> RemoveAllEmptyRooms()
        {
            var rooms = Rooms.Where(r => r.Value.Participants.Count <= 0)?.Select(r => r.Value);
            rooms?.ForEach(r => Remove(r.SessionCode));
            return rooms;
        }

        public void RemoveParticipantFromRoom(string code, string connectionId)
        {
            var room = GetOne(code);
            if (room != null)
            {
                room.Leave(connectionId);
                AddOrUpdate(code, room);

                RemoveAllEmptyRooms();
            }
        }

        public IEnumerable<OnlineMeetingRoom> GetContainedUserRooms(string connectionId)
        {
            var rooms = Rooms.Values.Where(r => r.Participants.Any(p => p.ConnectionId == connectionId));
            return rooms;
        }
    }
}
