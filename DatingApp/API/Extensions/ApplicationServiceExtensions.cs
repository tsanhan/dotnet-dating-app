using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    //1. the class must be static, no instances 
    public static class ApplicationServiceExtensions
    {
        //2. 'this' is what we want to extand, the instance of a class the extantion is applied on
        // we return whatever we want
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){
            // 3. cut-pasted from ConfigureServices in startup.cs
            services.AddScoped<ITokenService,TokenService>();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
            // go to startup.cs to use this
        }
    }
}