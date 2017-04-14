using System.Collections.ObjectModel;

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