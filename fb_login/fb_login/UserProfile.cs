using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fb_login
{
    public class UserProfile
    {
        public string id { get; set; }

        public string name { get; set; }

        public static UserProfile FromJson(string jsonText)
        {
            UserProfile profile = JsonConvert.DeserializeObject<UserProfile>(jsonText);
            return profile;
        }
    }
}
