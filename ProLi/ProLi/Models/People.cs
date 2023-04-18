using Azure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProLi.Models
{
    public class People
    {
        public People()
        {
            Events = new List<Event>();
        }

        [Key]
        public int Id { get; set; }
        public string GuestName { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Country { get; set; } 
        public string Phone { get; set; }   
        public string SpecialNote { get; set; }
        public string Note { get; set; }

        public ICollection<Event> Events { get; set; }

    }
}
