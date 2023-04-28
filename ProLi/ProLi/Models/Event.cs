using Azure;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace ProLi.Models
{
    public class Event
    {
        public Event()
        {
            People = new List<People>();
        }

        public int Id { get; set; }

        [Display(Name = "Megnevezés")]
        public string EventName { get; set; }
        [Display(Name = "Időpont")]
        public DateTime EventTime { get; set; }
        [Display(Name = "Helyszín")]
        public string Place { get; set; }
        [Display(Name = "Maximális létszám")]
        public int MaxPeople { get; set; }

        public ICollection<People> People { get; set; }
    }
}
