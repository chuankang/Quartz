using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Quartz.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult ExcelUpload()
		{
			return View();
		}

		/// <summary>
		/// EXCEL上传
		/// </summary>
		public ActionResult ExcelUploadSubmit(string excelTitle)
		{
			//取到上传域
			HttpPostedFileBase excelFile = Request.Files["excelFile"];

			if (null != excelFile)
			{
				//取到文件的名称
				string fileName = Path.GetFileName(excelFile.FileName);
				if (fileName != null && fileName.Equals(""))
				{
					//没有选择文件就上传的话，则跳回到上传页面
					return View("ExcelUpload");
				}
				string serverpath = Server.MapPath("/");

				string dir = serverpath + @"\Upload\";
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
				
				//parse the excel
				//save the excel content in the datatable 
				var dt = new DataTable();
				//新建一个DataTable，并指定列和列的类型
				dt.Columns.Add("IssuerName", Type.GetType("System.String"));
				dt.Columns.Add("Instury", Type.GetType("System.String"));
				dt.Columns.Add("PValue", Type.GetType("System.String"));
				dt.Columns.Add("IssuerRating", Type.GetType("System.String"));
				dt.Columns.Add("Nature", Type.GetType("System.String"));

				////解析上传的Excel
				////使用HSSF类，支持2007以前的excel（文件扩展名为xls），而XSSH支持07以后的（文件扩展名为xlsx）。

				//方式1：保存上传文件
				//excelFile.SaveAs(serverpath + @"\Upload\" + fileName);
				//FileStream inputStream = new FileStream(serverpath + @"\Upload\" + fileName, FileMode.Open);
				//XSSFWorkbook workbook = new XSSFWorkbook(inputStream);

				//方式2：文件流的形式读取，不保存excel到本地
				var workbook = new XSSFWorkbook(excelFile.InputStream);

				var sheet = workbook.GetSheetAt(0) as XSSFSheet;
				int rowNum = sheet.PhysicalNumberOfRows;
				for (var i = 1; i < rowNum; i++)
				{
					XSSFRow row = sheet.GetRow(i) as XSSFRow;
					int cellNum = row.PhysicalNumberOfCells;
					//DataTable创建新行
					DataRow newRow = dt.NewRow();
					for (int j = 0; j < cellNum; j++)
					{
						XSSFCell cell = row.GetCell(j) as XSSFCell;
						//给新建的行加列
						if (cell.CellType == CellType.Numeric)
						{
							newRow[j] = cell.NumericCellValue;
						}
						else
						{
							newRow[j] = cell.StringCellValue;
						}
					}
					// 新建的行加入到DataTable中
					dt.Rows.Add(newRow);
				}

				var products = new List<Product>();

				foreach (DataRow row in dt.Rows)
				{
					var product = new Product
					{
						IssuerName = StringUtil.NullToEmpty(row["IssuerName"]),
						Instury = StringUtil.NullToEmpty(row["Instury"]),
						PValue = StringUtil.NullToEmpty(row["PValue"]),
						IssuerRating = StringUtil.NullToEmpty(row["IssuerRating"]),
						Nature = StringUtil.NullToEmpty(row["Nature"])
					};

					products.Add(product);
				};


				ViewData["excelTitle"] = excelTitle;
				//存起来以便在前台显示
				ViewData["dt"] = dt;
			}

			return View("ExcelUpload");
		}

		public class Product
		{
			public string IssuerName { get; set; }
			public string Instury { get; set; }
			public string PValue { get; set; }
			public string IssuerRating { get; set; }
			public string Nature { get; set; }
		}

		public static class StringUtil
		{
			public static string NullToEmpty(object sValue)
			{
				if (sValue == DBNull.Value)
					return "--";
				return sValue == null ? "--" : ToDbc(sValue.ToString().Trim());
			}

			/// <summary>
			///     转半角
			/// </summary>
			/// <param name="input"></param>
			/// <returns></returns>
			private static string ToDbc(string input)
			{
				var c = input.ToCharArray();
				for (var i = 0; i < c.Length; i++)
				{
					if (c[i] == 12288)
					{
						c[i] = (char)32;
						continue;
					}
					if (c[i] > 65280 && c[i] < 65375)
						c[i] = (char)(c[i] - 65248);
				}
				return new string(c);
			}
		}
	}
}