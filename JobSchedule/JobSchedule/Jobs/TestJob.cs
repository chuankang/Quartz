using Commons.SqlHelpers;
using log4net;
using Models;
using Quartz;
using System;
using Newtonsoft.Json;

namespace JobSchedule.Jobs
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
			const string sql = "SELECT TOP 1 Code,Name FROM dbo.function_apps";
			var appList = DapperSqlHelper.GetList<TestModel>(sql);
			string json = JsonConvert.SerializeObject(appList);
			_logger.Info("测试Job开启--------------");
			_logger.Info($"输出{json}");
			Console.WriteLine($"{json}");
		}
	}
}
