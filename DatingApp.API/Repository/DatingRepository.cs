using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
           _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(x=> x.UserId == userId && x.IsMain);
            return photo;
        }

        public async Task<Photo> GetPhoto(int Id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(x=> x.Id == Id);
            return photo;
        }

        public async Task<User> GetUser(int Id)
        {
            var user = await _context.Users.Include(x=> x.Photos).FirstOrDefaultAsync(x=> x.Id == Id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
             var users = await  _context.Users.Include(x=> x.Photos).ToListAsync();
             return users;
        }

        public async Task<bool> SaveAllAsync()
        {
          return await  _context.SaveChangesAsync() > 0;
        }
    }
}