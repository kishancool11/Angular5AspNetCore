using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var UserId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo =  resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();
            var user = await repo.GetUser(UserId);
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync();
        }
    }
}