using Microsoft.EntityFrameworkCore.Query.Internal;
using ML_ASP.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _dbContext;

        public UnitOfWork(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            Submission = new SubmissionRepository(_dbContext);
            Account = new AccountRepository(_dbContext);
            Log = new LogRepository(_dbContext);
            Reminder = new ReminderRepository(_dbContext);
            QR = new QRRepository(_dbContext);
            Workload = new WorkloadRepository(_dbContext);
            WorkloadSubmissionList = new WorkloadSubmissionListRepository(_dbContext);
            Notification = new NotificationRepository(_dbContext);
            RequirementFile = new RequirementFileRepository(_dbContext);
            RequirementForm = new RequirementFormRepository(_dbContext);
            Overtime = new OvertimeRepository(_dbContext);
            ListItem = new _ListRepository(_dbContext);
            ListItem2 = new _ListRepository2(_dbContext);
        }

        public ISubmissionRepository Submission { get; private set; }
        public IAccountRepository Account { get; private set; }
        public ILogRepository Log { get; private set; }
        public IReminderRepository Reminder { get; private set; }
        public IQRRepository QR { get; private set; }
        public IWorkloadRepository Workload { get; private set; }
        public IWorkloadSubmissionListRepository WorkloadSubmissionList { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public IRequirementFileRepository RequirementFile { get; private set; }
        public IRequirementFormRepository RequirementForm { get; private set; }
        public IOvertimeRepository Overtime { get; private set; }
        public I_ListRepository ListItem { get; private set; }
        public I_ListRepository2 ListItem2 { get; private set; }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
