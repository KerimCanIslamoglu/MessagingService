using MessagingService.Business.Abstract;
using MessagingService.DataAccess.Abstract;
using MessagingService.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Business.Concrete
{
    public class ErrorLogsManager : IErrorLogsService
    {
        private IErrorLogsDal _errorLogsDal;
        public ErrorLogsManager(IErrorLogsDal errorLogsDal)
        {
            _errorLogsDal = errorLogsDal;
        }
        public void AddErrorLog(ErrorLogs errorLog)
        {
            _errorLogsDal.Create(errorLog);
        }
    }
}
