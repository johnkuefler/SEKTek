using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Services
{
    public class TaskCommentRepository
    {
        public async Task Add(ProjectTaskComment item)
        {
            await GlobalConfig.MobileService.GetTable<ProjectTaskComment>().InsertAsync(item);
        }

        public async Task Delete(ProjectTaskComment item)
        {
            await GlobalConfig.MobileService.GetTable<ProjectTaskComment>().DeleteAsync(item);
        }

        public async Task<ProjectTaskComment> Find(string id)
        {
            return await GlobalConfig.MobileService.GetTable<ProjectTaskComment>().LookupAsync(id);
        }

        public async Task<IEnumerable<ProjectTaskComment>> GetByCriteria(Expression<Func<ProjectTaskComment, bool>> predicate)
        {
            return await GlobalConfig.MobileService.GetTable<ProjectTaskComment>().Where(predicate).ToListAsync();
        }
    }
}
