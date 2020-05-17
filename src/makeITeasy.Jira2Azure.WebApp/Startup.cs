using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Attributed;
using AutoMapper;
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


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson()
                ;

            services.RegisterScheduler();

            services.AddScoped<FoobarJob>();

            services.AddSingleton(new JobSchedule(jobType: typeof(FoobarJob), cronExpression: "0/5 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();

            services.AddAutoMapper(ModelsAssembly.Get);

            services.AddMediatR(ServiceAssembly.Get);

            services.AddTransient<Func<ItemRepositoryDefinition, IItemRepository>>(serviceProvider => def =>
            {
                switch (def)
                {
                    case ItemRepositoryDefinition.Destination:
                        return serviceProvider.GetService<AzureDevopsRepository>();
                    default:
                        return null;
                }
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterType<JiraRepository>().Keyed<IItemRepository>("Source");
            builder.Register<AzureDevopsRepository>(x => new AzureDevopsRepository(Configuration.GetSection("ItemRepositories:AzureDevops").Get<AzureDevopsConfiguration>())).Keyed<IItemRepository>("Destination");

            builder.RegisterType<ItemService>().As<IItemService>().WithAttributeFilter();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
