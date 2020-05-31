using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.AttributeFilters;
using AutoMapper;
using makeITeasy.AzureDevops.Infrastructure;
using makeITeasy.AzureDevops.Infrastructure.ItemRepositories;
using makeITeasy.AzureDevops.Infrastructure.Jobs;
using makeITeasy.AzureDevops.Infrastructure.Scheduler;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Models.Configuration;
using makeITeasy.AzureDevops.Models.Scheduler;
using makeITeasy.AzureDevops.Services;
using makeITeasy.AzureDevops.Services.Domains.ItemDomain;
using makeITeasy.AzureDevops.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace makeITeasy.Jira2Azure.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson();

            services.RegisterScheduler();

            //register jobs
            services.AddScoped<FoobarJob>();
            services.AddSingleton(new JobSchedule(jobType: typeof(FoobarJob), cronExpression: "0/5 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();

            services.AddAutoMapper(ModelsAssembly.GetAssembly, InfrastructureAssembly.GetAssembly);
            services.AddMediatR(ServiceAssembly.GetAssembly);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<JiraItemRepository>()
                .Keyed<IItemRepository>("Source");

            builder.Register(x => 
                new AzureDevopsItemRepository(Configuration.GetSection("ItemRepositories:AzureDevops").Get<AzureDevopsConfiguration>(), x.Resolve<IMapper>()))
                .Keyed<IItemRepository>("Destination");

            builder.RegisterType<ItemService>().As<IItemService>().WithAttributeFiltering();

            builder.Register(x => new AzureDevopsSourceControlRepository(Configuration.GetSection("ItemRepositories:AzureDevops").Get<AzureDevopsConfiguration>(), x.Resolve<IMapper>())).As<ISourceControlRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
