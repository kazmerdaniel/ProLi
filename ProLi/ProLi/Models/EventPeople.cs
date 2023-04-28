
using System.ComponentModel.DataAnnotations;

namespace ProLi.Models
{
    public class EventPeople
    {
   
        public Event Event { get; set; }
        public int EventId { get; set; }   
        public People People { get; set; }
        public int PeopleId { get; set; }
    }
}
