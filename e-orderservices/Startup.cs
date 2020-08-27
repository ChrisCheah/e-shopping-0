using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using e_orderservices.Configuration;
using e_orderservices.Models;
using e_shoporderservices.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace e_orderservices
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

            services.Configure<OrderSettings>(
        Configuration.GetSection(nameof(OrderSettings)));
            services.AddSingleton<IOrderSettings>(sp =>
        sp.GetRequiredService<IOptions<OrderSettings>>().Value);
            services.AddSingleton<DataAccess>();
            services.AddControllers();
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
