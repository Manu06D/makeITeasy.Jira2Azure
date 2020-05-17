using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace makeITeasy.AzureDevops.Infrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public class FoobarJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
