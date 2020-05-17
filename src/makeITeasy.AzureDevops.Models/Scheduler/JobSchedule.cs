using System;
using System.Collections.Generic;
using System.Text;

namespace makeITeasy.AzureDevops.Models.Scheduler
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }

        public Type JobType { get; }
        public string CronExpression { get; }
    }
}
