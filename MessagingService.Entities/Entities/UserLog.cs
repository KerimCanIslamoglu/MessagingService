using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Entities
{
    public class UserLog
    {
        public int Id { get; set; }
        public string Activity { get; set; }
        public string ActivityDescription { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
