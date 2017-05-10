using ProjectTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Models
{
    public class Project : BaseDataObject
    {
        public string Color { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PercentComplete { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }


        private List<User> _resources;
        public List<User> Resources
        {
            get
            { return _resources; }
        }

        private List<ProjectTask> _tasks;
        public List<ProjectTask> Tasks
        {
            get
            { return _tasks; }
        }


        public async Task GetCompletionPercentage()
        {
           
            ProjectTaskRepository repository = new ProjectTaskRepository();
            IEnumerable<ProjectTask> tasks = await repository.GetByCriteria(x => x.ProjectID == this.Id);

            if (tasks.Count() > 0)
            {
                this.PercentComplete = Math.Round(tasks.Sum(x => x.PercentComplete) / tasks.Count()) + "%";
            }
            else
            {
                this.PercentComplete = "0%";
            }
        }

    }
}
