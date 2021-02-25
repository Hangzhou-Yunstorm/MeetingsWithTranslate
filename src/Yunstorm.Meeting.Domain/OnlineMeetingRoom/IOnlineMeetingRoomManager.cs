using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Yunstorm.Meeting.OnlineMeetingRoom
{
    public interface IOnlineMeetingRoomManager : ISingletonDependency
    {
        OnlineMeetingRoom GetOrCreate(string code);
        void AddOrUpdate(string code, OnlineMeetingRoom room);
        OnlineMeetingRoom GetOne(string sessionCode);
        Participant GetParticipantByConnectionId(string code, string connectionId);
        void RemoveParticipantFromRoom(string code, string connectionId);
        IEnumerable<OnlineMeetingRoom> GetContainedUserRooms(string connectionId);
    }
}
