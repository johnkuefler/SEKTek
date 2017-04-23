using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Services
{
    public class ProjectTaskRepository
    {
        public async Task Add(ProjectTask item)
        {
            await GlobalConfig.MobileService.GetTable<ProjectTask>().InsertAsync(item);
        }

        public async Task Delete(ProjectTask item)
        {
            await GlobalConfig.MobileService.GetTable<ProjectTask>().DeleteAsync(item);
        }

        public async Task<ProjectTask> Get(string id)
        {
            return await GlobalConfig.MobileService.GetTable<ProjectTask>().LookupAsync(id);
        }

        public async Task<IEnumerable<ProjectTask>> GetByCriteria(Expression<Func<ProjectTask, bool>> predicate)
        {
            return await GlobalConfig.MobileService.GetTable<ProjectTask>().Where(predicate).ToListAsync();
        }
    }
}
