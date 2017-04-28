using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Models
{
    public class ProjectTask : BaseDataObject
    {
        public string ProjectID { get; set; }

        public decimal PercentComplete { get; set; }
        public string PercentCompleteDisplay
        {
            get
            {
                return PercentComplete + "%";
            }
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        private List<User> _resources;
        public List<User> Resources
        {
            get
            { return _resources; }
        }
    }
}
