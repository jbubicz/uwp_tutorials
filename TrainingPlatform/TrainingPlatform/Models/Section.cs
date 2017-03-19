using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingPlatform.Models
{
    class Section
    {
        public int Id { get; set; }
        public int Course_id { get; set; }
        public int Section_order { get; set; }
        public string Title { get; set; }

        public ObservableCollection<Lesson> lessons { get; set; }
    }
}
