using ML_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories.IRepositories
{
	public interface ISubmissionRepository : IRepository<SubmissionModel>
	{
		public void Update(SubmissionModel model);
		public void ChangeApprovalStatus(int id, string status);
	}
}
