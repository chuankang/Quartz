using Quartz;

namespace JobSchedule.Jobs.TestJob
{
	//public class TestService : JobService<TestJob>
	//{
	//	protected override string JobName => "计算净值走势Job";
	//	protected override string GroupName => "基金";

	//	protected override ITrigger GetTrigger()
	//	{
	//		//步骤三：创建并配置一个触发器即trigger
	//		var trigger = TriggerBuilder.Create()
	//			.WithIdentity("触发器5秒执行一次", GroupName)//注意同一分组下不能有相同名称的触发器
	//			.WithCronSchedule("/5 * * ? * *")//10秒执行一次
	//			.Build();

	//		return trigger;
	//	}
	//}
}
