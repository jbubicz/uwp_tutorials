using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingPlatform
{
    class User
    {
        //private string fb_id = "0";

        public int Id { get; set; }
        public string Fb_id { get; set; }
        public int Role_id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Avatar { get; set; }
        public string About { get; set; }
        public int Points { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public User()
        {

        }

        public User fbUser(string fb_id)
        {
            User user = Database.getFBUserInfo(fb_id);
            return user;   
        }
    }
}
