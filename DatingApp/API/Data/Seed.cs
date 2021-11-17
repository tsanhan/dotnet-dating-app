using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        //logic to get the data from the json file to the db
        public static async Task/*1. no data return, only for the async*/ SeedUsers(DataContext context)
        {
            // 2. if the data is already in the db, then don't do anything
            if (await context.Users.AnyAsync()) return;

        
            var userData =  await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            //3. deserialize the json data into a list of AppUser objects
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in users)
            {
                //4. adding the users to our db
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;
                
                context.Users.Add(user); // 5. remember this is not the actual adding, only tracking the operation
            }

            await context.SaveChangesAsync(); // 6. actually saving the changes to the db

        }
    }
}