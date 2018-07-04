using Quartz;

namespace JobSchedule.Jobs.TestJob2
{
	//public class Test2Service : JobService<TestJob2>
	//{
	//	protected override string JobName => "计算收益率Job";

	//	protected override string GroupName => "基金";

	//	protected override ITrigger GetTrigger()
	//	{
	//		//步骤三：创建并配置一个触发器即trigger
	//		var trigger = TriggerBuilder.Create()
	//			.WithIdentity("触发器10秒执行一次", GroupName)//注意同一分组下不能有相同名称的触发器
	//			.WithCronSchedule("/10 * * ? * *")//10秒执行一次
	//			.Build();

	//		return trigger;
	//	}
	//}
}
