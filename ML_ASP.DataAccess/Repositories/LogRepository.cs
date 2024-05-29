using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models;
using ML_ASP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
    public class LogRepository : Repository<LogModel>, ILogRepository
    {
        private ApplicationDBContext _dbContext;

        public LogRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(LogModel logModel, string fileName, string fullName, int id)
        {
            var objFromDb = _dbContext.Logs.FirstOrDefault(u => u.Id == id);
            if (objFromDb != null)
            {
                objFromDb.LogImageUrl = fileName;
                objFromDb.FullName = fullName;
            }

            _dbContext.Update(objFromDb);
        }

		public void Update(LogModel logModel, string fileName, string fullName,string approvalStatus, int id, string fileId)
		{
			var objFromDb = _dbContext.Logs.FirstOrDefault(u => u.Id == id);
			if (objFromDb != null)
			{
				objFromDb.LogImageUrl = fileName;
                objFromDb.FullName = fullName;
                objFromDb.ApprovalStatus = approvalStatus;
                objFromDb.LogImageId = fileId;
            }

            _dbContext.Update(objFromDb);
		}

		public void Update(LogModel logModel)
        {
            _dbContext.Update(logModel);
        }

		public void ChangeApprovalStatus(int id, string status)
        {
			var objFromDb = _dbContext.Logs.FirstOrDefault(u => u.Id == id);
			if (objFromDb != null)
			{
				objFromDb.ApprovalStatus = status;
			}
		}
	}
}
