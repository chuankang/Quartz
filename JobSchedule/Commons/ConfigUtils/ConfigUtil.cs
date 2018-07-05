using System.Configuration;
using System.Data.SqlClient;

namespace Commons.ConfigUtils
{
	public class ConfigUtil
	{
		public static SqlConnection GetDevSqlConnection()
		{
			var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_Conn"].ConnectionString);
			con.Open();
			return con;
		}

		public static SqlConnection GetOwnSqlConnection()
		{
			var con = new SqlConnection(ConfigurationManager.ConnectionStrings["OwnCon"].ConnectionString);
			con.Open();
			return con;
		}
	}
}
