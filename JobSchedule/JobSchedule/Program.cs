using log4net.Config;
using System;
using System.IO;
using Topshelf;

namespace JobSchedule
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
			XmlConfigurator.ConfigureAndWatch(logCfg);

			HostFactory.Run(x =>
			{
				x.Service<JobManager>(t =>
				{
					//创建服务实例
					t.ConstructUsing(name => new JobManager());
					//服务开启
					t.WhenStarted(tc => tc.Start());
					//服务停止
					t.WhenStopped(tc => tc.Stop());
				});

				x.RunAsLocalSystem();//服务使用NETWORK_SERVICE内置帐户运行

				x.SetDisplayName("Fund基金数据计算");//名称
				x.SetDescription("计算基金收益率、净值走势、债券杠杆率...");//描述
				x.SetServiceName("Fund任务调度");//服务名称
			});
			Console.ReadLine();
		}
	}
}
