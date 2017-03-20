using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingPlatform.Models
{
    class Lesson
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Section_id { get; set; }
        public int Course_id { get; set; }
        public string Video { get; set; }
        public string Lesson_title { get; set; }
        public int Free { get; set; }
        public string Description { get; set; }
        public int Lesson_order { get; set; }
        public int IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
