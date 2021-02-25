using System;
using System.Collections.Generic;
using System.Text;

namespace Yunstorm.Meeting.MeetingRoom
{
    public class UserJoinViewModel : OnlineUserViewModel
    {
        public string SessionCode { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
