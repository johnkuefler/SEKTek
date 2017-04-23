using Microsoft.WindowsAzure.MobileServices;
using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker
{
    public static class GlobalConfig
    {
        public static void Initialize()
        {
            currentUser = new User
            {
                Id = "1",
                FirstName = "Jackson",
                LastName = "Smith",
                EmailAddress = "jackson@sektek.com",
                Password = "test",
                PictureURL = "",
                Title = "Project Manager",
            };
        }

        private static User currentUser;
        public static User CurrentUser
        {
            get
            {
                return currentUser;
            }
        }

        public static MobileServiceClient MobileService =
         new MobileServiceClient(
           "YOUR_AZURE_CONNECTION_STRING"
        );
    }
}
