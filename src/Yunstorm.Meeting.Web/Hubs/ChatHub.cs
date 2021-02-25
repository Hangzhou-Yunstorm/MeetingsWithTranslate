using System;
using System.Linq;
using System.Threading.Tasks;
using Masuit.Tools;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.ObjectMapping;
using Yunstorm.Meeting.Meeting;
using Yunstorm.Meeting.MeetingRoom;
using Yunstorm.Meeting.OnlineMeetingRoom;
using Yunstorm.Meeting.Web.Hubs.Models;

namespace Yunstorm.Meeting.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IObjectMapper _mapper;
        private readonly IOnlineMeetingRoomManager _onlineMeetingRoomManager;
        private readonly IMeetingRoomManager _meetingRoomManager;
        public ChatHub(IObjectMapper mapper,
            IMeetingRoomManager meetingRoomManager,
            IOnlineMeetingRoomManager onlineMeetingRoomManager)
        {
            _mapper = mapper;
            _meetingRoomManager = meetingRoomManager;
            _onlineMeetingRoomManager = onlineMeetingRoomManager;
        }

        /// <summary>
        /// 发起会话
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<CreateResult> Create(OnlineUserViewModel user)
        {
            if (!user.IsValid())
            {

            }

            user.ConnectionId = Context.ConnectionId;

            // 创建一个会议室
            var room = await _meetingRoomManager.CreateAsync(user.Nickname);
            if (room == null)
            {
                return new CreateResult(null, false, "创建失败，请重试！");
            }

            // 加入到在线会议室列表中
            var onlineRoom = _onlineMeetingRoomManager.GetOrCreate(room.SessionCode);
            var participant = _mapper.Map<OnlineUserViewModel, Participant>(user);
            onlineRoom.Join(participant);
            _onlineMeetingRoomManager.AddOrUpdate(room.SessionCode, onlineRoom);

            // 把创建者加入到会议室中
            await Groups.AddToGroupAsync(user.ConnectionId, room.SessionCode);

            return new CreateResult(room.SessionCode);
        }

        /// <summary>
        /// 加入会话
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<JoinResult> Join(UserJoinViewModel user)
        {
            if (!user.IsValid())
            {

            }
            user.ConnectionId = Context.ConnectionId;

            // 从在线会议室中获取此会议室
            var onlineRoom = _onlineMeetingRoomManager.GetOne(user.SessionCode);
            // 从所有会议室获取到此会议室
            var room = await _meetingRoomManager.GetBySessionCodeAsync(user.SessionCode);

            // 同时存在
            if (onlineRoom == null || null == room)
            {
                return new JoinResult(false, "找不到会话。请检查此代码是否正确，以及当前会话是否属于活动状态。");
            }

            // 加入到此会议室中
            var participant = _mapper.Map<UserJoinViewModel, Participant>(user);
            onlineRoom.Join(participant);
            _onlineMeetingRoomManager.AddOrUpdate(room.SessionCode, onlineRoom);

            await Groups.AddToGroupAsync(user.ConnectionId, onlineRoom.SessionCode);

            // 通知其他人有新用户加入
            await Clients.GroupExcept(onlineRoom.SessionCode, user.ConnectionId).SendAsync("UserJoined", participant);

            // 添加加入会议的消息
            var message = new Message(null, "System", $"{user.Nickname} 加入了会议", "zh-cn");
            room.AppendMessage(message);
            await _meetingRoomManager.UpdateAsync(room);

            // 发送通知消息
            var msg = _mapper.Map<Message, ReturnMessage>(message);
            await Clients.Group(onlineRoom.SessionCode).SendAsync("ReceiveMessage", msg);

            return new JoinResult(true, "加入成功");
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Send(MessageViewModel model)
        {
            if (!model.IsValid())
            {

            }

            // 获取发送者的信息
            var code = model.SessionCode;
            var connectionId = Context.ConnectionId;

            var sender = _onlineMeetingRoomManager.GetParticipantByConnectionId(code, connectionId);
            var room = await _meetingRoomManager.GetBySessionCodeAsync(code);

            if (sender != null && room != null)
            {
                // 持久化消息
                var message = new Message(sender.Id, sender.Nickname, model.Content, sender.Language, model.IsVoice, model.VoiceFile);
                room.AppendMessage(message);
                await _meetingRoomManager.UpdateAsync(room);

                // 发送消息到群组中
                var result = _mapper.Map<Message, ReturnMessage>(message);
                await Clients.Group(code).SendAsync("ReceiveMessage", result);
            }
        }

        /// <summary>
        /// 退出聊天室
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            var connectionId = Context.ConnectionId;

            // 获取用户所在会议室
            var rooms = _onlineMeetingRoomManager.GetContainedUserRooms(connectionId);
            if (rooms != null)
            {
                foreach (var room in rooms)
                {
                    // 通知用户离开
                    var user = room.Participants.FirstOrDefault(p => p.ConnectionId == connectionId);
                    if (user != null)
                    {
                        _onlineMeetingRoomManager.RemoveParticipantFromRoom(room.SessionCode, connectionId);

                        await Clients.Group(room.SessionCode).SendAsync("UserLeft", user);

                        // 持久化离开消息
                        var message = new Message(null, "System", $"{user.Nickname} 离开了会议", "zh-cn");
                        var r = await _meetingRoomManager.GetBySessionCodeAsync(room.SessionCode);
                        if (r != null)
                        {
                            r.AppendMessage(message);
                            await _meetingRoomManager.UpdateAsync(r);
                        }

                        // 发送通知消息
                        var msg = _mapper.Map<Message, ReturnMessage>(message);
                        await Clients.Group(room.SessionCode).SendAsync("ReceiveMessage", msg);
                    }
                }
            }
        }

        /// <summary>
        /// 处理重新连接
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var request = Context.GetHttpContext().Request;
            var userId = request.Query["userId"].ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }
            var connectionId = Context.ConnectionId;


        }
    }
}
