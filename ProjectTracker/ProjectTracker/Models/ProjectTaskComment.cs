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
        public DateTime DateTime { get; set; }


        private User user;


        public void LoadUser(User u)
        {
            this.user = u;
        }

        public User GetUser()
        {
            return user;
        }
    }
}
