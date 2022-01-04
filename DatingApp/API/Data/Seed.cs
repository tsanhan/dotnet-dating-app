using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        //1. We get a user manager now the use Identity, no need to the DataContext now
        // * we;ll use the user manager insted
        public static async Task SeedUsers(/*DataContext context*/ UserManager<AppUser> userManager)
        {
            //2. fix this to use userManager
            // * we can se how much methods does this Object provide
            // * we use Users only
            if (await userManager.Users.AnyAsync()) return;

        
            var userData =  await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();

                //3. change here to use userManager
                // context.Users.Add(user);
                await userManager.CreateAsync(user, "Pa$$w0rd");
                // the ability to add a password to the user later on.
                // you want simple passwords? 
                // remove other settings for a complex passwords (in IdentityServiceExtensions.cs, password configuration)


            }
            //4. the user manager takes care of the saving of changes to the database
            // await context.SaveChangesAsync(); 

            //5. no wee need to pass UserManager to this class constructor,
            // * go to Program.cs
        }
    }
}