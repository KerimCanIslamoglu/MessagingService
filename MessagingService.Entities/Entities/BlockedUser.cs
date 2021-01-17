using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Entities
{
    public class BlockedUser
    {
        public int Id { get; set; }
        public string BlockingUserName { get; set; }
        public string BlockedUserName { get; set; }
        public DateTime BlockedAt { get; set; }
    }
}
