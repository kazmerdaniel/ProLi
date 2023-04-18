using Azure;
using Microsoft.Extensions.Logging;

namespace ProLi.Models
{
    public class Event
    {
        public Event()
        {
            People = new List<People>();
        }

        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime EventTime { get; set; }
        public string Place { get; set; }
        public int MaxPeople { get; set; }

        public ICollection<People> People { get; set; }
    }
}
