using Quartz;

namespace JobSchedule
{
	public abstract class JobService<T> where T : IJob
	{
		/// <summary>
		/// Job名称
		/// </summary>
		protected abstract string JobName { get; }
		/// <summary>
		/// Job组名
		/// </summary>
		protected abstract string GroupName { get; }
		/// <summary>
		/// 触发器名称
		/// </summary>
		protected abstract string TriggerName { get; }
		/// <summary>
		/// 定时时间
		/// </summary>
		protected abstract string WithCronSchedule { get; }

		private IJobDetail GetJobDetail()
		{
			//步骤二：创建一个具体的Job (具体的Job需要单独在一个文件中执行)
			var job = JobBuilder.Create<T>()
				.WithIdentity(JobName, GroupName)
				.Build();
			return job;
		}

		private ITrigger GetTrigger()
		{
			//步骤三：创建并配置一个触发器即trigger
			var trigger = TriggerBuilder.Create()
				.WithIdentity(TriggerName, GroupName)//注意同一分组下不能有相同名称的触发器
				.WithCronSchedule(WithCronSchedule)
				.Build();

			return trigger;
		}
		//protected abstract ITrigger GetTrigger();

		public void AddJobToSchedule(IScheduler scheduler)
		{
			//步骤四：将Job和trigger加入到作业调度池中
			scheduler.ScheduleJob(GetJobDetail(), GetTrigger());
		}
	}
}
