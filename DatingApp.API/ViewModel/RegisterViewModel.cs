using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.ViewModel
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }

        [Required]
        public string UserName {get;set;}

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage= "You must specify a password between 4 and 10 characters")]
        public string Password {get;set;}

        [Required]
        public string Gender {get;set;}

        [Required]
        public string KnownAs {get;set;}

        [Required]
        public DateTime DateOfBirth {get;set;}

        [Required]
        public string City {get;set;}

        [Required]
        public string Country {get;set;}
 
        public DateTime Created {get;set;}
        public DateTime LastActive {get;set;}
        
        
    }
}