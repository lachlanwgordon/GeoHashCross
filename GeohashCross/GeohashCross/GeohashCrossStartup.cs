using System;
using GeohashCross.Models;
using Microsoft.Extensions.DependencyInjection;
using Shiny;
using Shiny.Jobs;

namespace GeohashCross
{
    public class GeohashCrossStartup : Shiny.ShinyStartup
    {
        public GeohashCrossStartup()
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var job = new JobInfo(typeof(LocationNotificationJob));
            services.RegisterJob(job);
            services.UseNotifications();
        }
    }
}
