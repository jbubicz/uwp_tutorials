using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingPlatform
{
    class Course
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Category { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }        
        public string ImgUrl { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

       
    }


}
