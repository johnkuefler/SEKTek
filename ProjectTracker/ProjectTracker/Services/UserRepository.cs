using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Services
{
    class UserRepository
    {
        public async Task Add(User item)
        {
            await GlobalConfig.MobileService.GetTable<User>().InsertAsync(item);
        }

        public async Task Delete(User item)
        {
            await GlobalConfig.MobileService.GetTable<User>().DeleteAsync(item);
        }

        public async Task<User> Find(string id)
        {
            return await GlobalConfig.MobileService.GetTable<User>().LookupAsync(id);
        }

        public async Task<List<User>> GetByCriteria(Expression<Func<User, bool>> predicate)
        {
            return await GlobalConfig.MobileService.GetTable<User>().Where(predicate).ToListAsync();
        }

        public async Task<User> Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
