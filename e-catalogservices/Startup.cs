using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using e_shopCatalogServices.EFContext;
using e_shopCatalogServices.Repository;
using e_shopCatalogServices.Configuration;
using Consul;

namespace e_catalogservices
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
            var connection = @"Data Source=DESKTOP-55AGI0I\SQLEXPRESS;Initial Catalog=EcommerceDB;Persist Security Info=True;User ID=sa;Password=vignesh";
           services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));
            services.AddScoped< ICatalogRepository, CatalogRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();


            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, ConsulHostedService>();
            services.Configure<ConsulConfig>(Configuration.GetSection("ConsulConfig"));

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = Configuration["ConsulConfig:Address"];
                consulConfig.Address = new Uri(address);
            }));

            services.AddSingleton<Func<IConsulClient>>(p => () => new ConsulClient(consulConfig =>
            {
                var address = Configuration["ConsulConfig:Address"];
                consulConfig.Address = new Uri(address);
            }));

            services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
