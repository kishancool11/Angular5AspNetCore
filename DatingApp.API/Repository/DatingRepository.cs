using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models.Data;
using DatingApp.API.ViewModel;
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

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
             var users = _context.Users.Include(x=> x.Photos).AsQueryable();
             users = users.Where(u => u.Id != userParams.UserId);
             users = users.Where(u => u.Gender == userParams.Gender);
             
             if(!(userParams.MinAge == 18 && userParams.MaxAge == 99))
             {
                 users = users.Where(x=> x.DateOfBirth.CalculateAge() >= userParams.MinAge 
                                        && x.DateOfBirth.CalculateAge() <= userParams.MaxAge);
             }

             return await PagedList<User>.CreateAsync(users,userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
          return await  _context.SaveChangesAsync() > 0;
        }
    }
}