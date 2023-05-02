
using System.ComponentModel.DataAnnotations;

namespace ProLi.Models
{
    public class EventPeople
    {
   
        public int Id { get; set; }
        public Event Event { get; set; }
        public int EventsId { get; set; }   
        public People People { get; set; }
        public int PeopleId { get; set; }
    }
}
