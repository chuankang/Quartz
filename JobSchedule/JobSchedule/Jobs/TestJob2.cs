using Quartz;
using log4net;
using System;

namespace JobSchedule.Jobs
{
	public class TestJob2 : JobService<TestJob2>, IJob
	{
		private readonly ILog _logger = LogManager.GetLogger(typeof(TestJob2));
		protected override string JobName => "计算收益率Job";
		protected override string GroupName => "基金";
		protected override string TriggerName => "触发器10秒执行一次";
		protected override string WithCronSchedule => "/10 * * ? * *";

		public void Execute(IJobExecutionContext context)
		{
			_logger.Info("测试Job2开启--------------");
			Console.WriteLine("hello2");
		}
	}
}
