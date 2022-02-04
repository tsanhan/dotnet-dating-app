using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {   
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;
            
           
            var userId = resultContext.HttpContext.User.GetUserId();

            //1. we'll get the IUnitOfWork from the service provider and use it
            var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user = await uow.UserRepository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;//2. change to UtxNow, to be consistent with other dates 
            await uow.Complete();

            //2. also onw other thing, we'll remove all _context.SaveChangesAsync(); in the app except in the UOW.
            // * it;s not the repository job anymore, it's the unit of work job
            // * we have one in MessageRepository.cs, go there and remove it

        }
    }
}
