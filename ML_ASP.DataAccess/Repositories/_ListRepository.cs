using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models;
using ML_ASP.Models.Models.UserDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
    public class _ListRepository : Repository<ListModel>, I_ListRepository
    {
        private ApplicationDBContext _dbContext;

        public _ListRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public void UpdateSettings(int optionToDelete, string timestart, string timeends, string daystart, string dayends)
        {
            var objFromDb = _dbContext.ListName.FirstOrDefault(u => u.id == optionToDelete);
            if (objFromDb != null)
            {
                objFromDb.TimeIn = timestart;
                objFromDb.TimeOut = timeends;
                objFromDb.WorkingDaysStart = daystart;
                objFromDb.WorkingDaysEnds = dayends;
            }
        }
	}
}
