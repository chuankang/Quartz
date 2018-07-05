using Commons.Utils;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Commons.SqlHelpers
{
	public class DapperSqlHelper
	{
		/// <summary>
		/// 查询获取集合对象
		/// </summary>
		public static List<T> GetList<T>(string sql, object param = null)
		{
			using (var conn = ConfigUtil.GetDevSqlConnection())
			{
				return conn.Query<T>(sql, param).ToList();
			}
		}

		public static DataTable GetDateTable(string sql, object param = null)
		{
			using (var conn = ConfigUtil.GetDevSqlConnection())
			{
				var dt = new DataTable();
				var reader = conn.ExecuteReader(sql);
				dt.Load(reader);
				return dt;
			}
		}

		public static DataSet GetDataSet(string sql, params SqlParameter[] sqlParameters)
		{
			var ds = new DataSet();
			using (var conn = ConfigUtil.GetDevSqlConnection())
			{
				var cmd = new SqlCommand(sql, conn);
				if (sqlParameters != null)
				{
					foreach (var parameter in sqlParameters)
					{
						cmd.Parameters.Add(parameter);
					}
				}
				var da = new SqlDataAdapter(cmd);
				da.Fill(ds);
			}
			return ds;
		}

		public static int Execute(string sql, object param = null)
		{
			using (var conn = ConfigUtil.GetDevSqlConnection())
			{
				return conn.Execute(sql, param);
			}
		}
	}
}
