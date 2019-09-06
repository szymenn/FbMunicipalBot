using System;
using FbRestaurantsBot.Configuration;
using FbRestaurantsBot.Extensions;
using FbRestaurantsBot.Helpers;
using FbRestaurantsBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FbRestaurantsBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.Configure<FacebookSettings>(Configuration.GetSection(Constants.FacebookSettings));
            services.Configure<ZomatoSettings>(Configuration.GetSection(Constants.ZomatoSettings));

            services.AddHttpClient<IMessengerClient, MessengerClient>(config =>
            {
                config.BaseAddress = new Uri("https://graph.facebook.com/v2.6/me/messages");
            });
            services.AddHttpClient<IZomatoApiClient, ZomatoApiClient>(config =>
            {
                config.BaseAddress = new Uri("https://developers.zomato.com/api/v2.1/");
            });
            
            services.AddScoped<IMessengerService, MessengerService>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCustomExceptionHandler();

            app.UseMvc();
        }
    }
}