using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.ViewModel
{
    public class PhotoForCreateViewModel
    {

        public string Url {get;set;}
        public IFormFile File { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public string PublicId { get; set; }
        public PhotoForCreateViewModel()
        {
            DateAdded = DateTime.Now;
        }

        
    }
}