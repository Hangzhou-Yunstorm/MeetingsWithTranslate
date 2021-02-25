using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yunstorm.Meeting.Web.Hubs.Models
{
    public class MethodResultBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public MethodResultBase(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    public class CreateResult : MethodResultBase
    {
        public string SessionCode { get; set; }
        public CreateResult(string sessionCode, bool success = true, string message = null) : base(success, message)
        {
            SessionCode = sessionCode;
        }
    }

    public class JoinResult : MethodResultBase
    {
        public JoinResult(bool success, string message) : base(success, message)
        {

        }
    }


}
