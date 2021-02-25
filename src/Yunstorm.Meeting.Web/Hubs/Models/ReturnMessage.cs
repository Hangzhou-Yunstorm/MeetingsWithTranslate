using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yunstorm.Meeting.Web.Hubs.Models
{
    public class ReturnMessage
    {
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
        public bool IsVoice { get; set; }
        public string VoiceFile { get; set; }
    }
}
