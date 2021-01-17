using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingService.Models
{
    public class SendMessageModel
    {
        public string Message { get; set; }
        public string UserTo { get; set; }
    }
}
