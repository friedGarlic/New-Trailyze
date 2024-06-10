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
    public class _ListRepository2 : Repository<ListModel2>, I_ListRepository2
    {
        private ApplicationDBContext _dbContext;

        public _ListRepository2(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
