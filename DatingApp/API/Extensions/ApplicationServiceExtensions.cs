using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            // services.AddScoped<ILikesRepository, LikesRepository>();    //1. no need for that
            // services.AddScoped<IMessageRepository, MessageRepository>();//2. no need for that
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            // services.AddScoped<IUserRepository, UserRepository>();      //3. no need for that
            services.AddScoped<IUnitOfWork, UnitOfWork>();                 //4. add this
                                                                             // * so now every controller will have an instance of the UOF
                                                                             // * and that means one instance of data context per controller call
                                                                             // * no matter the repository usage needed in this controller. 
                                                                             
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;

            //5. so we don't need the 'SaveAllAsync' in the repositories.
            //  * they not responsible for saving changes to the database, the UOF does.
            //6. go to IUserRepository.cs and remove the SaveAllAsync method
        }
    }
}