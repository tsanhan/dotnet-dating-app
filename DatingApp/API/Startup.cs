using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using API.Extensions;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {

            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 1. cutting to create the AddApplicationServices extantion method
            // services.AddScoped<ITokenService,TokenService>();
            // services.AddDbContext<DataContext>(options =>
            // {
            //     options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            // });

            services.AddApplicationServices(_config); //2. after creating AddApplicationServices as an extantion method, we can use it.


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddCors();
            //2. to do the same with Identity Service Extensions well cut this and go to IdentityServiceExtensions.cs

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options => {// 2. configure parameters 
            //     options.TokenValidationParameters = new TokenValidationParameters{
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),
            //         ValidateIssuer = false, // the api server 
            //         ValidateAudience = false // the angular app
            //         //we can add validations against those ☝ too but our main concern is the token 
            //     };
            // });
            
            //3. use AddIdentityServices from IdentityServiceExtensions.cs
            services.AddIdentityServices(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(policy =>
            policy
            .AllowAnyHeader() // Allow Any Header (like authentication related)
            .AllowAnyMethod()// Allow Any Method (GET/POST/PUT/PATCH)
            .WithOrigins("https://localhost:4200") // our frontend
            );

            // 3. the order is important
            // Authentication come before Authorization, Cors comes before Authentication
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
