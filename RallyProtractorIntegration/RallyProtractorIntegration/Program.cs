using System;
using System.Linq;
using System.Threading.Tasks;
using Topshelf;
using Quartz;
using Quartz.Impl;
using RallyProtractorIntegration.Config;

namespace RallyProtractorIntegration
{
    class Program
    {
        static void Main(string[] args)
        {

            HostFactory.Run(svc =>
            {
                svc.Service<JobScheduler>(job =>
                {
                    job.WhenStarted(tc => tc.Start());
                    job.WhenStopped(tc => tc.Stop());
                });
                svc.RunAsLocalService();

                svc.SetDescription("This service will genearte a report by comparing rally and  prortractor testcases and get  those test cases which is not implemented in protractor and send mail");
                svc.SetDisplayName("RallyProtractorIntegration");
                svc.SetServiceName("RallyProtractorIntegrationService");
                svc.StartAutomatically();
            });
        }

        public class JobScheduler
        {
            public void Start()
            {

                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                IJobDetail job = JobBuilder.Create<MainService>().Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule
                      (s =>
                         s.WithIntervalInSeconds(5)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(22, 33))
                      )
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }

            public void Stop() { Console.WriteLine("Service Stop at {0}", DateTime.Now); }

        }

        public class MainService : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                var configSection = RallyConfig.WorkItems;
                if (configSection != null)
                {

                    Parallel.ForEach(configSection.Cast<WorkItemElement>(), workitem =>
                    {
                        var rallyProtractor = new RallyProtractorIntegration(workitem);

                        rallyProtractor.ExecuteWorkItem().Wait();
                    });
                }
            }
        }
    }
}
