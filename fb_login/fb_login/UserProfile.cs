using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fb_login
{


    public class Rootobject
    {
        public Datum[] data { get; set; }
        public Paging paging { get; set; }
        public Summary summary { get; set; }

        public static Rootobject FromJson(string jsonText)
        {
            Rootobject profile = JsonConvert.DeserializeObject<Rootobject>(jsonText);
            return profile;
        }
    }

    public class Paging
    {
        public Cursors cursors { get; set; }
    }

    public class Cursors
    {
        public string before { get; set; }
        public string after { get; set; }
    }

    public class Summary
    {
        public int total_count { get; set; }
    }

    public class Datum
    {
        public string name { get; set; }
        public string id { get; set; }
    }





    //public string id { get; set; }

    //public string name { get; set; }


}
