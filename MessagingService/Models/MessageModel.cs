using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingService.Models
{
    public class MessageModel
    {
        public string MessageDetail { get; set; }
        public string UserFrom { get; set; }
        public string UserTo { get; set; }
        public DateTime SendedAt { get; set; }
    }
}
