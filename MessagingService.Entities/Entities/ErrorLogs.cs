using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Entities
{
    public class ErrorLogs
    {
        public int Id { get; set; }
        public string ErrorDetail { get; set; }
        public string MethodName { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
