using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Organization.Services.Customer.Interfaces;
using Organization.Services.Customer.Repository;
using Organization.Services.Customer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Host
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
            services.AddControllers();

            // Add framework services.
            services.AddMvc();

            var connectionString = Configuration["DbContextSettings:ConnectionString"];
            services.AddDbContext<CustomerContext>(
                opts => opts.UseNpgsql(connectionString)
            );

            services.AddTransient<IDbConnection>((sp) =>
            new NpgsqlConnection(Configuration["DbContextSettings:ConnectionString"]));

            services
                .AddTransient<IContactValidationService, ContactValidationService>()
                .AddTransient<ICustomerManagementService, CustomerManagementService>()
                .AddTransient<ICustomerNoteManagementService, CustomerNoteManagementService>()
                .AddTransient<IQueueService, QueueService>()
                .AddTransient<IRepositoryService, RepositoryService>()
                .AddTransient<IDatabaseTranslator, DatabaseTranslator>();
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
