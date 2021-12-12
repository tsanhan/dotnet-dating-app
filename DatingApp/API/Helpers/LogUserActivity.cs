using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    //1. implement IAsyncActionFilter
    public class LogUserActivity: IAsyncActionFilter
    {
        //2. context: is the context of the action being executed, we can see that we have inside there (like Result, ActionArguments, etc...)
        //3. next: what will happen after the action excecuted, we use it to execute the action and then do something after it's been excecuted
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //3. so we want to get the context after the action excecuted (look @ return from ActionExecutionDelegate)
            // ok so we have the context before (context) and we have the context after
            
            //4. so we want the user to do what he wanted to to in our API and only then do another thing.
            //5. we'll start with excecuting the delegate and capturing the context returning   
            var resultContext = await next();

            //6. we need to check if the user authenticated
            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return; // if the user setup a token && we authenticated it => true

            //7. if the user is authenticated, we can update the lastActive property
            // so first we get the username from the token
            var username = resultContext.HttpContext.User.GetUsername(); 

            //8. we reqest an instance of our users repository as a service 
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();

            //9. get the user and update LastActive.
            var user = await repo.GetUserByUserNameAsync(username); //11. not a good solution, 
                                                                    // the method get the photos (no need) and searching by username (not indexed)
                                                                    // better use GetUserByIdAsync (id is indexed)
                                                                    // we'll fix this in short time 
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync();

            //10. this action filter is acting as a service so we need to add it to application services
            // go to ApplicationServiceExtensions.cs
        }
    }
}
