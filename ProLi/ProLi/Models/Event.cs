namespace ProLi.Models
{
    public class
        Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime EventTime { get; set; }
        public string Place { get; set; }
        public int MaxPeople { get; set; }
        public List<People> Guests { get; set; } 
    }
}
