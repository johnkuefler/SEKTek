using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Services
{
    public class ProjectRepository
    {
        public async Task Add(Project item)
        {
            await GlobalConfig.MobileService.GetTable<Project>().InsertAsync(item);
        }

        public async Task Delete(Project item)
        {
            await GlobalConfig.MobileService.GetTable<Project>().DeleteAsync(item);
        }

        public async Task<Project> Get(string id)
        {
            return await GlobalConfig.MobileService.GetTable<Project>().LookupAsync(id);
        }

        public async Task<IEnumerable<Project>> GetByCriteria(Expression<Func<Project, bool>> predicate)
        {
            return await GlobalConfig.MobileService.GetTable<Project>().Where(predicate).ToListAsync();
        }
    }
}
