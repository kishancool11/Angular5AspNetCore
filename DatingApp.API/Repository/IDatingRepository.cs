using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models.Data;

namespace DatingApp.API.Repository
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAllAsync();
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int Id);
    }
}