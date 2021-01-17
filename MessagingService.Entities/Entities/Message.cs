using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageDetail { get; set; }
        public string UserFrom { get; set; }
        public string UserTo { get; set; }
        public DateTime SendedAt { get; set; }
    }
}
