using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Products.API.DataAccess;
using Products.API.DataAccess.Repositories;

namespace Products.API
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
            services.AddControllers();

            // For test purposes
            //services.AddDbContext<AppDbContext>(builder => 
            //    builder.UseInMemoryDatabase("eSzop"));

            var connectionString = Configuration.GetConnectionString("SqlServer");
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var hostnameToResolve = connectionStringBuilder.DataSource.Split(",").First();
            var hostIp = Dns.GetHostEntry(hostnameToResolve).AddressList.First();
            connectionString = connectionString.Replace(hostnameToResolve, hostIp.ToString());

            services.AddDbContext<AppDbContext>(builder =>
                builder.UseSqlServer(connectionString));

            services.AddScoped<IProductRepository, ProductRepository>();
        }

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
