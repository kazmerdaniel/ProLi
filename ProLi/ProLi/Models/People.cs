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
            Office = new List<Office>();
        }

        [Key]
        public int Id { get; set; }
        [Display(Name = "Név")]
        public string GuestName { get; set; }
        [Display(Name = "...")]
        public string? Title { get; set; }
        [Display(Name = "Cím")]
        public string? Address { get; set; }
        [Display(Name = "...")]
        public string? Organization { get; set; }
        [Display(Name = "E-mail")]
        public string? Email { get; set; }
        [Display(Name = "Kép elérési útja")]
        public string? Image { get; set; }
        [Display(Name = "Ország")]
        public string? Country { get; set; }
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }
        [Display(Name = "Bizalmas megjegyzés")]
        public string? SpecialNote { get; set; }
        [Display(Name = "Megjegyzés")]
        public string? Note { get; set; }

        public ICollection<Event> Events { get; set; }
        public ICollection<Office> Office { get; set; }

    }
}
