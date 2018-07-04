using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace JobSchedule
{
	public class ScheduleBase
	{
		private static IScheduler _scheduler;
		public static IScheduler Scheduler
		{
			get
			{
				if (_scheduler != null)
				{
					return _scheduler;
				}
				//配置CrystalQuartz远程管理（web界面）
				var properties = new NameValueCollection
				{
					["quartz.scheduler.instanceName"] = "基金数据作业监控系统",
					// 设置线程池
					["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
					["quartz.threadPool.threadCount"] = "5",
					["quartz.threadPool.threadPriority"] = "Normal",
					// 远程输出配置
					["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz",
					["quartz.scheduler.exporter.port"] = "8008",
					["quartz.scheduler.exporter.bindName"] = "QuartzScheduler",
					["quartz.scheduler.exporter.channelType"] = "tcp"
				};

				//步骤一：创建作业调度池
				var schedulerFactory = new StdSchedulerFactory(properties);
				_scheduler = schedulerFactory.GetScheduler();

				return _scheduler;
			}
		}

		public static void AddSchedule<T>(JobService<T> service) where T : IJob
		{
			service.AddJobToSchedule(Scheduler);
		}
	}
}
