using ML_ASP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories.IRepositories
{
    public interface IWorkloadSubmissionListRepository : IRepository<WorkloadSubmissionList_Model>
    {
        public void IsSubmitted(bool isSubmitted, int id);
    }
}
