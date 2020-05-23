using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using makeITeasy.Jira2Azure.WebApp.Models;
using Quartz;
using System.Text;
using Quartz.Impl.Matchers;

namespace makeITeasy.Jira2Azure.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISchedulerFactory schedulerFactory;

        public HomeController(ILogger<HomeController> logger, ISchedulerFactory schedulerFactory)
        {
            _logger = logger;
            this.schedulerFactory = schedulerFactory;
        }

        public async Task<IActionResult> Index()
        {
            var scheduler = await schedulerFactory.GetScheduler();
            var jobGroups = await scheduler.GetJobGroupNames();
            var builder = new StringBuilder().AppendLine().AppendLine();

            foreach (var group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = await scheduler.GetJobKeys(groupMatcher);

                foreach (var jobKey in jobKeys)
                {
                    var detail = await scheduler.GetJobDetail(jobKey);
                    var triggers = await scheduler.GetTriggersOfJob(jobKey);

                    foreach (ITrigger trigger in triggers)
                    {
                        builder.AppendLine(string.Format("Job: {0}", jobKey.Name));
                        builder.AppendLine(string.Format("Description: {0}", detail.Description));
                        var nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            builder.AppendLine(string.Format("Next fires: {0}", nextFireTime.Value.LocalDateTime));
                        }
                        var previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            builder.AppendLine(string.Format("Previously fired: {0}", previousFireTime.Value.LocalDateTime));
                        }
                        var simpleTrigger = trigger as ISimpleTrigger;
                        if (simpleTrigger != null)
                        {
                            builder.AppendLine(string.Format("Repeat Interval: {0}", simpleTrigger.RepeatInterval));
                        }
                        builder.AppendLine();
                    }
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
