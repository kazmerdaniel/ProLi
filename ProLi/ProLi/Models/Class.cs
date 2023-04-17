using Microsoft.AspNetCore.Identity;

namespace ProLi.Models
{
    public class IdentityUserWithRole: IdentityUser 
    {
        public string UserRole { get; set; }
        public string Password  { get; set; }   
      
    }

}
