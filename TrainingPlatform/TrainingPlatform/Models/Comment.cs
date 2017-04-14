using System;

namespace TrainingPlatform.Models
{
    class Comment
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public string Author { get; set; }
        public int Lesson_id { get; set; }
        public string Value { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        //public ObservableCollection<Comment> comments { get; set; }
    }
}
