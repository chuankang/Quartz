using JobSchedule.Jobs;

namespace JobSchedule
{
	public class JobManager : IJobService
	{
		public void Start()
		{
			//步骤四：将job和trigger加入到作业调度池中
			ScheduleBase.AddSchedule(new TestJob());
			//ScheduleBase.AddSchedule(new TestJob2());

			//步骤五：开启调度
			ScheduleBase.Scheduler.Start();
		}

		public void Stop()
		{
			ScheduleBase.Scheduler.Shutdown(true);
		}
	}
}
