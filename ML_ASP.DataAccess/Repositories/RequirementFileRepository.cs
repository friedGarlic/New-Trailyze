using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
    public class RequirementFileRepository : Repository<RequirementFile_Model>, IRequirementFileRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public RequirementFileRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void UpdateStatus(int id, string newUpdateStatus)
        {
            var objFromDb = _dbContext.RequirementFile.FirstOrDefault(u => u.Id == id);
            if (objFromDb != null)
            {
                objFromDb.ApprovalStatus = newUpdateStatus;
            }

		}
    }
}
