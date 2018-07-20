using System.Collections.Generic;
using System.Linq;

namespace Quartz.Models
{
	public class ResultEntity
	{
		public int code { get; set; }
		public string Message { get; set; }
	}

	public class ResultEntity<T> : ResultEntity
	{
		public T Data { get; set; }
	}


	public class PageResultEntity<T> : ResultEntity
	{
		/// <summary>
		/// 当前页数
		/// </summary>
		public long CurrentPage { get; set; }

		/// <summary>
		/// 总页数
		/// </summary>
		public long TotalPages { get; set; }

		/// <summary>
		/// 总数量
		/// </summary>
		public long count { get; set; }

		/// <summary>
		/// 每页数量
		/// </summary>
		public long ItemsPerPage { get; set; }

		/// <summary>
		/// 数据内容
		/// </summary>
		public List<T> data { get; set; }

		public dynamic Extension { get; set; }

	}

	public class PageRequestEntity<TRequest>
	{
		public int CurrentPage { get; set; }

		public int PageSize { get; set; }

		public TRequest data { get; set; }

		/// <summary>
		/// 执行分页操作，并返回对应的PageResultEntity
		/// </summary>
		public PageResultEntity<TResult> WithPagedItems<TResult>(IEnumerable<TResult> items, string message = default(string))
		{
			var list = items as List<TResult> ?? items.ToList();
			int totalItems = list.Count;
			list = list.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
			return AsPageResult(list, totalItems, message);
		}

		/// <summary>
		/// 不执行分页，直接转换为PageResultEntity
		/// </summary>
		public PageResultEntity<TResult> AsPageResult<TResult>(IEnumerable<TResult> items, int totalItems, string message = default(string))
		{
			var itemList = items as List<TResult> ?? items.ToList();

			var pageResult = new PageResultEntity<TResult>
			{
				code = 0,
				Message = message,
				CurrentPage = CurrentPage,
				data = itemList,
				ItemsPerPage = itemList.Count,
				count = totalItems,
				TotalPages = totalItems % PageSize == 0 ? totalItems / PageSize : totalItems / PageSize + 1
			};

			return pageResult;
		}
	}
}
