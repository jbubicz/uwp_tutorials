using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingPlatform.Models
{
    class SectionsList : Section
    {
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