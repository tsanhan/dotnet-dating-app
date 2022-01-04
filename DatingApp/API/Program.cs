using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();

                await context.Database.MigrateAsync();
                //1. in order to see if the database appling the migration, we'll skip the seed process
                // await Seed.SeedUsers(context);
                //2. just do a `dotnet run` (this will also update the database).
                // * if you have any issues (if not runing .net 5), try to delete the migrations folder, and creating a new migration.
                // * the table structure of the database should modified (applying the migration, adding tables, removing fields)
                // * if we'll look at the AspNetUsers table we'll see new and old columns and data.
                // * one issue here, we still will have to drop and recreate the database, why?
                // * because the password-hash data came from our generator and not from Identity, so it won't recognize them. 
                
                //3. back to README.md
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration");
            }
            
            //4. we start the host
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
