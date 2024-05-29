using ML_ASP.Models;
using ML_ASP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories.IRepositories
{
    public interface ILogRepository : IRepository<LogModel>
    {
        public void Update(LogModel logModel);
		public void Update(LogModel logModel, string fileName, string fullName, int id);
		public void Update(LogModel logModel, string fileName, string fullName, string approvalStatus, int id, string fileId);
        public void ChangeApprovalStatus(int id, string status);
	}
}
