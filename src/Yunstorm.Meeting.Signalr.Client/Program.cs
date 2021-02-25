using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yunstorm.Meeting.Signalr.Client
{
    class Program
    {
        static string BaseAddress = "http://10.0.1.46:19782";

        /// <summary>
        /// 参与者
        /// </summary>
        static ConcurrentQueue<Participant> Participants = new ConcurrentQueue<Participant>();
        static HubConnection Connection;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // 用户 Id
            var id = Guid.NewGuid().ToString("N");
            // 用户名称
            string name;
            // 用户语言
            string language = "zh-cn";
            // 会话 Code
            var code = string.Empty;

            var command = string.Empty;

            Connection = new HubConnectionBuilder()
                .WithUrl($"{BaseAddress}/chat?userId={id}")
                .Build();

            // 接收到信息时
            Connection.On<Message>("ReceiveMessage", message =>
            {
                Console.WriteLine($"【{message.SenderName}】({message.Language}): {message.Content}");
            });

            // 有用户加入时
            Connection.On<Participant>("UserJoined", participant =>
            {
                Participants.Enqueue(participant);
            });

            // 开始连接
            await Connection.StartAsync();


            do
            {
                Console.WriteLine("请输入你的名字：");
                name = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(name));

            do
            {
                Console.WriteLine("请输入命令:(c) 创建\t\t(j) 加入");
                command = Console.ReadLine().ToLower();

                if (command == "c")
                {
                    code = await CreateAsync(id, name, language);
                    break;
                }
                else if (command == "j")
                {
                    code = await JoinAsync(id, name, language);
                    break;
                }
            } while (true);

            if (!string.IsNullOrWhiteSpace(code))
            {
                var msg = string.Empty;
                do
                {
                    Console.WriteLine("请输入消息");
                    msg = Console.ReadLine();

                    await Connection.InvokeAsync("Send", new SendMessageModel { SessionCode = code, Content = msg });

                } while (!string.IsNullOrWhiteSpace(msg));
            }

            await Connection.StopAsync();
            Console.ReadKey();
        }

        private static async Task<string> CreateAsync(string id, string name, string language)
        {
            // 创建会话 返回创建结果
            var user = new { Id = id, Nickname = name, Language = language };
            var createResult = await Connection.InvokeAsync<CreateResult>("Create", user);

            if (!createResult.Success)
            {
                Console.WriteLine(createResult.Message);
                return string.Empty;
            }

            Console.WriteLine($"创建成功，Code 为 {createResult.SessionCode}");
            return createResult.SessionCode;
        }

        private static async Task<string> JoinAsync(string id, string name, string language)
        {
            string code;

            do
            {
                Console.WriteLine("请输入会话代码：");
                code = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(code));

            // 加入到已存在的会话中
            var user = new { SessionCode = code, Id = id, Nickname = name, Language = language };
            var joinResult = await Connection.InvokeAsync<JoinResult>("Join", user);
            Console.WriteLine(joinResult.Message);
            if (joinResult.Success)
            {
                await FetchParticipants(code);
                return code;
            }
            return null;
        }

        /// <summary>
        /// 获取历史消息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static async Task<List<Message>> GetHistoryMessages(string code)
        {
            var http = new HttpClient();
            var response = await http.GetAsync($"{BaseAddress}/api/meeting/messages?sessionCode={code}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Message>>(json);
            }
            return null;
        }

        /// <summary>
        /// 获得在线参与者
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static async Task FetchParticipants(string code)
        {
            var http = new HttpClient();
            var response = await http.GetAsync($"{BaseAddress}/api/meeting/participants?sessionCode={code}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var participants = JsonConvert.DeserializeObject<List<Participant>>(json);
                participants?.ForEach(p => Participants.Enqueue(p));
            }
        }
    }

    /// <summary>
    /// 发送的消息
    /// </summary>
    public class SendMessageModel
    {
        /// <summary>
        /// 会话码
        /// </summary>
        public string SessionCode { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否为语音
        /// </summary>
        public bool IsVoice { get; set; } = false;
        /// <summary>
        /// 语音文件
        /// </summary>
        public string VoiceFile { get; set; }
    }

    /// <summary>
    /// 接收的消息
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 发送者Id
        /// </summary>
        public string SenderId { get; set; }
        /// <summary>
        /// 发送者名称
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 是否为语音
        /// </summary>
        public bool IsVoice { get; set; } = false;
        /// <summary>
        /// 语音文件
        /// </summary>
        public string VoiceFile { get; set; }
    }

    /// <summary>
    /// 参与者
    /// </summary>
    public class Participant
    {
        /// <summary>
        /// 参与者Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 连接Id
        /// </summary>
        public string ConnectionId { get; set; }
    }

    /// <summary>
    /// Invoke 方法后的返回值基础类型
    /// </summary>
    public class MethodResultBase
    {
        /// <summary>
        /// 指示是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

    }

    /// <summary>
    /// 创建会话结果
    /// </summary>
    public class CreateResult : MethodResultBase
    {
        /// <summary>
        /// 会话码
        /// </summary>
        public string SessionCode { get; set; }

    }

    /// <summary>
    /// 加入会话结果
    /// </summary>
    public class JoinResult : MethodResultBase
    {
    }
}
