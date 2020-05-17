using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace makeITeasy.AzureDevops.Infrastructure.Scheduler
{
    public static class RegisterSchedulerExtension
    {
        public static void RegisterScheduler(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzJobRunner>();
        }
    }
}
