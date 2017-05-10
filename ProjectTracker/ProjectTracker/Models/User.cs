using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Models
{
    public class User : BaseDataObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            { return FirstName + " " + LastName; }
        }
        public string Title { get; set; }
        public string PictureURL { get; set; }
        public string EmailAddress { get; set; }
        public string Skills { get; set; }
        public string Password { get; set; }

        public string GetRole()
        {
            if (FirstName == "Jackson")
            {
                return "Admin";
            }
            else
            {
                return "User";
            }
        }
    }
}
