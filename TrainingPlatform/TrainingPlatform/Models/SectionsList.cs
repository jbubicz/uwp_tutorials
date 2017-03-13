using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingPlatform.Models
{
    class SectionsList
    {
        public int Id { get; set; }
        public int Course_id { get; set; }
        public int Section_order { get; set; }
        public string Title { get; set; }

        public SectionsList()
        {
        }

        public SectionsList(int course_id)
        {
            this.Course_id = course_id;
        }

        public ObservableCollection<SectionsList> Sections { get; set; } = new ObservableCollection<SectionsList>();
    }
}