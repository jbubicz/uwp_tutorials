using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingPlatform
{

    public class FBUserRootobject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public FBUserPicture picture { get; set; }

        public static FBUserRootobject FromJson(string jsonText)
        {
            FBUserRootobject profile = JsonConvert.DeserializeObject<FBUserRootobject>(jsonText);
            return profile;
        }
    }

    public class FBUserPicture
    {
        public Data data { get; set; }
    }

    public class FBUserData
    {
        public bool is_silhouette { get; set; }
        public string url { get; set; }
    }


}
