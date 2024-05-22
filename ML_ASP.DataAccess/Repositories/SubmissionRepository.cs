using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
	public class SubmissionRepository : Repository<SubmissionModel>, ISubmissionRepository
	{
		private ApplicationDBContext _dbContext;

		public SubmissionRepository(ApplicationDBContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public void Update(SubmissionModel model) {
			var objFromDb = _dbContext.Submissions.FirstOrDefault(u => u.Id == model.Id);
            if (objFromDb != null)
			{
				objFromDb.ApprovalStatus = model.ApprovalStatus;
				objFromDb.Details = model.Details;
				objFromDb.DueStatus = model.DueStatus;
				objFromDb.Date = model.Date;
				objFromDb.Details = model.Details;
				objFromDb.FileName = model.FileName;
				objFromDb.Name = model.Name;
			}
        }

		public void ChangeApprovalStatus(int id, string status){
			var objFromDb = _dbContext.Submissions.FirstOrDefault(u => u.Id == id);
			if (objFromDb != null)
			{
				objFromDb.ApprovalStatus = status;
			}
		}
    }
}
