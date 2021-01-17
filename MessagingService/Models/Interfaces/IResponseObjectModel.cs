using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingService.Models.Interfaces
{
    public interface IResponseObjectModel<T>
    {
        bool Success { get; set; }
        string Message { get; set; }
        T Response { get; set; }
    }
}
