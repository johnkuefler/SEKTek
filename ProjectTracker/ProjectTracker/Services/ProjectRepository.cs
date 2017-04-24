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

        public async Task<Project> Find(string id)
        {
            return await GlobalConfig.MobileService.GetTable<Project>().LookupAsync(id);
        }

        public async Task<IEnumerable<Project>> GetByCriteria(Expression<Func<Project, bool>> predicate)
        {
            return await GlobalConfig.MobileService.GetTable<Project>().Where(predicate).ToListAsync();
        }


        public async Task<List<User>> GetProjectResources(string projectID)
        {
            UserRepository userRepository = new UserRepository();

            List<User> output = new List<User>();
            List<UserProject> userProjects = await GlobalConfig.MobileService.GetTable<UserProject>().Where(rec => rec.ProjectID == projectID).ToListAsync();

            foreach (UserProject userProject in userProjects)
            {
                output.Add(await userRepository.Find(userProject.UserID));
            }

            return output;
        }



        public async Task AddProjectResource(string projectID, string userID)
        {
            UserProject up = new UserProject
            {
                ProjectID = projectID,
                UserID = userID
            };

            await GlobalConfig.MobileService.GetTable<UserProject>().InsertAsync(up);
            
        }
    }
}
