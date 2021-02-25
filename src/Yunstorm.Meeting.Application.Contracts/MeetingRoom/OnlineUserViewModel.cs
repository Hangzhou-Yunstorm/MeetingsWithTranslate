using System;
using System.Collections.Generic;
using System.Text;
using Yunstorm.Meeting.ViewModels;

namespace Yunstorm.Meeting.MeetingRoom
{
    public class OnlineUserViewModel : ViewModelBase
    {
        public string ConnectionId { get; set; }
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Language { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
