using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AutoMapper;
using Bluebird.Auth;
using id4.Data;
using id4.Models;

namespace id4
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .AddNewtonsoftJsonCamelCase();

            services.ConfigureIIS();
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContexts(Configuration);
            services.AddCustomIdentityServer(Configuration);
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = Configuration["Google:ClientId"];
                    options.ClientSecret = Configuration["Google:ClientSecret"];
                });
            services.AddScoped<IUserSession,FakeSession>();
            services.AddScoped<SeedService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddRepositories();
        }

        public void Configure(IApplicationBuilder app, SeedService seed, IServiceProvider provider)
        {
            app.UsePathBase(Configuration["BasePath"]); // DON'T FORGET THE LEADING SLASH!
            app.UseDefaultErrorHandling(Environment);
           
            seed.Seed();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}