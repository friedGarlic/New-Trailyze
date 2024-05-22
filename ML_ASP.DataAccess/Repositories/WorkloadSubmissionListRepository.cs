using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
    public class WorkloadSubmissionListRepository : Repository<WorkloadSubmissionList_Model>, IWorkloadSubmissionListRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public WorkloadSubmissionListRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void IsSubmitted(bool isSubmitted, int id)
        {
            var objFromDb = _dbContext.WorkloadsSubmissionList.FirstOrDefault(x => x.Id == id);
            if (objFromDb != null)
            {
                objFromDb.IsSubmitted = isSubmitted;
            }
        }
    }
}
