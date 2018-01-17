using System.Threading.Tasks;
using DatingApp.API.Models.Data;

namespace DatingApp.API.Repository
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}