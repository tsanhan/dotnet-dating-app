using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        // 1. here we injecting our configuration into the startup class.
        // 1. I don't really like it, so we'll follow a slightly different way of specifying dependency injection...
        // 1. other developers are into this way too, it's not just me
        private readonly IConfiguration _config;
        // 4. When we construct this class, the IConfiguration is being injected into this class when it's constructed,
        public Startup(IConfiguration config) //2. shorted the name and ctrl+. 'init field from parameter'
        {

            /*3.*/
            _config = config;
            // Configuration = configuration;

            //4. I don;t want to remove the 'this' and retype the variables so:
            //4.1. for the _ retyping: in settings, add _ to 'Private Member Prefix'
            //4.2. for removing 'this', uncheck 'Use This For Ctor Assignments'
            //4.3. check the works by deleting line 30 and 24. follow point 2. again
        }

        // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // the ordering here is not so important
            // don't use DbContext here 


            services.AddDbContext<DataContext>(options =>
            { // this is a lambda expression - very common if you want to pass expression as a parameter
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
