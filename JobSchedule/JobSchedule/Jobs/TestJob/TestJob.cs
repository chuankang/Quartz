using log4net;
using Quartz;
using System;

namespace JobSchedule.Jobs.TestJob
{
	public class TestJob : JobService<TestJob>, IJob
	{
		private readonly ILog _logger = LogManager.GetLogger(typeof(TestJob));
		protected override string JobName => "计算净值走势Job";
		protected override string GroupName => "基金";
		protected override string TriggerName => "触发器5秒执行一次";
		protected override string WithCronSchedule => "/5 * * ? * *";

		public void Execute(IJobExecutionContext context)
		{
			_logger.Info("测试Job开启--------------");
			Console.WriteLine("hello");
		}
	}
}
