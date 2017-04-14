using Newtonsoft.Json;

namespace TrainingPlatform
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
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Picture picture { get; set; }
    }

    public class Picture
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public bool is_silhouette { get; set; }
        public string url { get; set; }
    }

    // nie wiem czy to bedzie potrzebne?
    //public class FriendsManager
    //{
    //    public static void GetFriends(ObservableCollection<Rootobject> friendsList)
    //    {
    //        friendsList.Clear();
    //        var allFriends = getFriendsList();
    //    }

    //    private static List<Rootobject> getFriendsList()
    //    {
    //        var friendsList = new List<Rootobject>();
    //        return friendsList;
    //    }
    //}
}
