using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly HMACHelper _HMACHelper;
        public AuthRepository(DataContext context)
        {
            _context = context;
            _HMACHelper = new HMACHelper();
        }

        public async  Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(x=> x.Photos).FirstOrDefaultAsync(x=> x.Name == username);

            if(username == null)
            {                
                return null;
            }
            
            if(!_HMACHelper.VerifyPasswordHash(password,user.Password,user.PasswordSalt))
            {
                return null;
            } 
            return user;
        }


        public async Task<User> Register(User user, string password)
        {
             byte[] passwordHash, passwordSalt;
             _HMACHelper.CreatePasswordHash(password,out passwordHash,out passwordSalt);

             user.Password = passwordHash;
             user.PasswordSalt = passwordSalt;

             await _context.Users.AddAsync(user);
             await _context.SaveChangesAsync();

             return user;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x=> x.Name == username))
            {
                return true;
            } 
            return false;
        }
    }
}