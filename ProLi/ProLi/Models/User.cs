using System.ComponentModel.DataAnnotations;

namespace ProLi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Jelszó")]
        public string Password { get; set; }
        [Display(Name = "Jog")]
        public string UserRole { get; set; }
 
    }
}
