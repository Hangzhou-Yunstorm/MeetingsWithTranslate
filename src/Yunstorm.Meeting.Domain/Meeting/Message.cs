using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace Yunstorm.Meeting.Meeting
{
    public class Message : Entity<long>
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public string SenderId { get; private set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string SenderName { get; private set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; private set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendingTime { get; private set; }
        /// <summary>
        /// 是否为语音消息
        /// </summary>
        public bool IsVoice { get; private set; }
        /// <summary>
        /// 语言文件
        /// </summary>
        public string VoiceFile { get; private set; }

        public Message(string senderId, string senderName, string content, string language, bool isVoice = false, string voiceFile = null)
        {
            SenderId = senderId;
            SenderName = senderName;
            Content = content;
            Language = language;
            IsVoice = isVoice;
            VoiceFile = voiceFile;

            SendingTime = DateTime.UtcNow;
        }
    }
}
