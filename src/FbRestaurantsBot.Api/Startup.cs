using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FbRestaurantsBot.Core.Configuration;
using FbRestaurantsBot.Core.Interfaces;
using FbRestaurantsBot.Core.Services;
using FbRestaurantsBot.Infrastructure.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Constants = FbRestaurantsBot.Core.Helpers.Constants;

namespace FbRestaurantsBot.Api
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
            services.AddHttpClient<IZomatoClient, ZomatoClient>(config =>
            {
                config.BaseAddress = new Uri("https://developers.zomato.com/api/v2.1/");
            });
            
            services.AddScoped<IMessengerService, MessengerService>();
            services.AddScoped<ILoggerAdapter, LoggerAdapter>();
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
            
            app.UseMvc();
        }
    }
}