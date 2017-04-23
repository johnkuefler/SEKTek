using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Models
{
    public class UserProject : BaseDataObject
    {
        public string UserID { get; set; }
        public string ProjectID { get; set; }
    }

    public class UserProjectTask : BaseDataObject
    {
        public string UserID { get; set; }
        public string ProjectTaskID { get; set; }

    }
}
