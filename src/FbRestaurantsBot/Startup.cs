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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.Configure<FacebookSettings>(Configuration.GetSection(Constants.FacebookSettings));
            services.Configure<ZomatoSettings>(Configuration.GetSection(Constants.ZomatoSettings));

            services.AddHttpClient<IMessengerClient, MessengerClient>();
            services.AddHttpClient<IZomatoApiClient, ZomatoApiClient>();
            services.AddScoped<IMessengerService, MessengerService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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