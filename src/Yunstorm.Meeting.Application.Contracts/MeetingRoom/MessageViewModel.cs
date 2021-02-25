using System;
using System.Collections.Generic;
using System.Text;
using Yunstorm.Meeting.ViewModels;

namespace Yunstorm.Meeting.MeetingRoom
{
    public class MessageViewModel : ViewModelBase
    {
        public string SessionCode { get; set; }
        public string Content { get; set; }
        public bool IsVoice { get; set; } = false;
        public string VoiceFile { get; set; }

        public MessageViewModel(string content)
        {
            Content = content;
        }

        public MessageViewModel()
        {

        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
