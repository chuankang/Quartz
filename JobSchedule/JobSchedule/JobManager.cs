using JobSchedule.Jobs.TestJob;
using JobSchedule.Jobs.TestJob2;

namespace JobSchedule
{
	public class JobManager : IJobService
	{
		public bool Start()
		{
			//步骤四：将job和trigger加入到作业调度池中
			ScheduleBase.AddSchedule(new TestJob());
			ScheduleBase.AddSchedule(new TestJob2());

			//步骤五：开启调度
			ScheduleBase.Scheduler.Start();
			return true;
		}

		public bool Stop()
		{
			ScheduleBase.Scheduler.Shutdown(true);
			return true;
		}
	}
}
