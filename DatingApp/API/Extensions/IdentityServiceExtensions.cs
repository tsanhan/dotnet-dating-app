using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config){
            
            services.AddIdentityCore<AppUser>(ops => {
                ops.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false, 
                    ValidateAudience = false  
                };

                //1. we adding something to the options of how we send the token to the client.
                // * our api constroller will keep using the authentcation header as before.
                // * SignalR will use a query string parameter called 'access_token'
                options.Events = new JwtBearerEvents{
                // * we'll configure the flow on every message event received by the hub.
                    OnMessageReceived = context => {
                        // we want the token from the query string
                        var accessToken = context.Request.Query["access_token"];
                        // check where is this request comming to
                        var path = context.HttpContext.Request.Path;
                        // if the accessToken and the the message it going to "/hubs" 
                        //  * fix 'bubs' to 'hubs' in Startup.cs
                        //  * this path ned to match the path of the hub as we configured in Startup.cs 
                        if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs")){
                            // we'll extract the token from the query string and place it in the context for the [authorize] attribute to use it.
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
                //2. all the above will allow our client to send the token as a query string parameter.
                //3. ow because of the way we can now send credentials as a query string parameter, we need to update a little CORS policy.
                // * go to Startup.cs
            });

            services.AddAuthorization(options => {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
            });
            return services;
        }
    }
}