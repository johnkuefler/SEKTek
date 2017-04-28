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

        public async Task<ProjectTask> Find(string id)
        {
            return await GlobalConfig.MobileService.GetTable<ProjectTask>().LookupAsync(id);
        }

        public async Task<IEnumerable<ProjectTask>> GetByCriteria(Expression<Func<ProjectTask, bool>> predicate)
        {
            return await GlobalConfig.MobileService.GetTable<ProjectTask>().Where(predicate).ToListAsync();
        }

        public async Task<List<User>> GetAssignedResources(string taskID)
        {
            UserRepository userRepository = new UserRepository();

            List<User> output = new List<User>();

            List<ProjectTaskAssignment> assignments = await GlobalConfig.MobileService.GetTable<ProjectTaskAssignment>().Where(rec => rec.ProjectTaskID == taskID).ToListAsync();

            foreach (ProjectTaskAssignment assignment in assignments)
            {
                output.Add(await userRepository.Find(assignment.UserID));
            }

            return output;
        }

        public async Task AddTaskResource(string taskID, string userID)
        {
            ProjectTaskAssignment assignment = new ProjectTaskAssignment
            {
                ProjectTaskID = taskID,
                UserID = userID
            };

            await GlobalConfig.MobileService.GetTable<ProjectTaskAssignment>().InsertAsync(assignment);
        }

        public async Task<List<ProjectTaskComment>> GetComments(string taskID)
        {
            TaskCommentRepository commentRepository = new TaskCommentRepository();
            UserRepository userRepository = new UserRepository();

            List<ProjectTaskComment> comments = await GlobalConfig.MobileService.GetTable<ProjectTaskComment>().Where(rec => rec.ProjectTaskID == taskID).ToListAsync();

            foreach (ProjectTaskComment comment in comments)
            {
                comment.LoadUser(await userRepository.Find(comment.UserID));
            }

            return comments;
        }

        public async Task AddTaskComment(string userID, string taskID, string comment)
        {
            ProjectTaskComment newComment = new ProjectTaskComment
            {
                ProjectTaskID = taskID,
                UserID = userID,
                Comment = comment,
                DateTime = DateTime.Now,
            };

            await GlobalConfig.MobileService.GetTable<ProjectTaskComment>().InsertAsync(newComment);
        }
    }
}
