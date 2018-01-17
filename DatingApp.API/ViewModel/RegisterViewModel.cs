using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName {get;set;}

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage= "You must specify a password between 4 and 10 characters")]
        public string Password {get;set;}
    }
}