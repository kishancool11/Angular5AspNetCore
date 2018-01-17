using System.Collections.Generic;
using DatingApp.API.Helpers;
using Newtonsoft.Json;

namespace DatingApp.API.Models.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context= context;
        }

        public void SeedUsers()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
            HMACHelper hMAC = new HMACHelper();
            var userData = System.IO.File.ReadAllText("Models/Data/UserSeedData.json");
            
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach(var user in users)
            {
                byte[] passwordHash, passwordSalt;   
                hMAC.CreatePasswordHash("password", out passwordHash, out passwordSalt);
                user.PasswordSalt = passwordSalt;
                user.Password = passwordHash;
                user.Name = user.Name.ToLower();
                _context.Users.AddRange(user);
            }

            _context.SaveChanges();
        }
    }
}