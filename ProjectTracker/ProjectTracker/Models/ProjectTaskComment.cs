using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Models
{
    public class ProjectTaskComment : BaseDataObject
    {
        public string ProjectTaskID { get; set; }
        public string UserID { get; set; }
        public string Comment { get; set; }
    }
}
