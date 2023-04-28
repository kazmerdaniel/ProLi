
using System.ComponentModel.DataAnnotations;

namespace ProLi.Models
{
    public class EventPeople
    {
        [Display(Name = "Rendezvény")]
        public Event Event { get; set; }
        public int EventId { get; set; }
        [Display(Name = "Személyek")]
        public People People { get; set; }
        public int PeopleId { get; set; }
    }
}
