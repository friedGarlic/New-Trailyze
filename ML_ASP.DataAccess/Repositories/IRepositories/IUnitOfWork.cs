using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        ISubmissionRepository Submission { get; }

        IAccountRepository Account { get; }

        ILogRepository Log { get; }

        IReminderRepository Reminder { get; }

        IQRRepository QR { get; }

        IWorkloadRepository Workload { get; }
        
        IWorkloadSubmissionListRepository WorkloadSubmissionList { get; }

        INotificationRepository Notification { get; }

        IRequirementFileRepository RequirementFile { get; }

        IRequirementFormRepository RequirementForm { get; }

        IOvertimeRepository Overtime { get; }

        I_ListRepository ListItem { get; }

        I_ListRepository2 ListItem2 { get; }

        void Save();
    }
}
