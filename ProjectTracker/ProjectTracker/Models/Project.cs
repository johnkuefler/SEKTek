using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Models
{
    public class Project : BaseDataObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

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

    }
}
