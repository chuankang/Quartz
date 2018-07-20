namespace Quartz.Models
{
	public class ResultEntity
	{
		public int code { get; set; }
		public int count { get; set; }
		public string Message { get; set; }
	}

	public class ResultEntity<T> : ResultEntity
	{
		public T data { get; set; }
	}
}
