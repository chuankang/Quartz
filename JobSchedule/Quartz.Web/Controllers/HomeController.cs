using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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


		/// <summary>
		/// EXCEL上传 选择上传
		/// </summary>
		public ActionResult ExcelUploadSubmit2(string excelTitle)
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

				//方式2：文件流的形式读取，不保存excel到本地
				var workbook = new XSSFWorkbook(excelFile.InputStream);

				//var sheet = workbook.GetSheetAt(0) as XSSFSheet;

				ISheet sheet = workbook.GetSheetAt(0);

				int totalRowCount = sheet.LastRowNum; //总行数
				int headerRowIndex = 0,
					issuerNameCellIndex = -1,
					insturyCellIndex = -1,
					pValueCellIndex = -1,
					issuerRatingCellIndex = -1,
					natureCellIndex = -1,
					dateCellIndex = -1;

				IRow headerRow = null;
				while (headerRowIndex < totalRowCount)
				{
					headerRow = sheet.GetRow(headerRowIndex);
					if (headerRow.GetCell(5) != null && headerRow.GetCell(5).ToString() == "P值时间")
					{
						break;
					}

					headerRowIndex += 1;
				}

				if (headerRowIndex < totalRowCount)
				{
					for (var i = 0; i < 6; i++)
					{
						if (headerRow == null)
						{
							continue;
						}

						var cell = headerRow.GetCell(i);
						var headerText = cell == null ? string.Empty : cell.ToString();

						if (headerText == "主体名称")
						{
							issuerNameCellIndex = i;
						}
						else if (headerText == "行业")
						{
							insturyCellIndex = i;
						}
						else if (headerText == "P值")
						{
							pValueCellIndex = i;
						}
						else if (headerText == "主体评级")
						{
							issuerRatingCellIndex = i;
						}
						else if (headerText == "企业性质")
						{
							natureCellIndex = i;
						}
						else if (headerText == "P值时间")
						{
							dateCellIndex = i;
						}

					}

					var productList = new List<Product>();

					if (issuerNameCellIndex > -1 && insturyCellIndex > -1 && pValueCellIndex > -1
						&& issuerRatingCellIndex > -1 && natureCellIndex > -1 && dateCellIndex > -1)
					{
						var pvaluedate = sheet.GetRow(headerRowIndex + 1).GetCell(dateCellIndex).DateCellValue;

						for (int i = headerRowIndex+1; i <= totalRowCount; i++)
						{
							IRow currentRow = sheet.GetRow(i);
							
							productList.Add(new Product
							{
								FundId = 11500005,
								PvalueId = 1,
								IssuerName = currentRow.GetCell(issuerNameCellIndex).StringCellValue,
								Instury = currentRow.GetCell(insturyCellIndex).StringCellValue,
								PValue = currentRow.GetCell(pValueCellIndex).NumericCellValue.ToString(),
								IssuerRating = currentRow.GetCell(issuerRatingCellIndex).StringCellValue,
								Nature = currentRow.GetCell(natureCellIndex).StringCellValue,
							});
						}
					}
					
					ViewData["dt"] = productList;

					var exportData = productList.Select(t => new[]
					{
						t.IssuerName,
						t.Nature,
						t.IssuerRating
					});

					//Excel名称
					string filename = ExportToFile("", "", HeaderText, exportData);

					//导出excel


				}
			}

			return View("ExcelUpload");
		}


		/// <summary>
		/// 下载Excel
		/// </summary>
		/// <param name="fileName">上面接口返回的Excel路径</param>
		/// <returns></returns>
		public FileStreamResult DownloadFile(string fileName)
		{
			string fullName = "文件路径" + fileName;
			FileStream fileStream = System.IO.File.OpenRead(fullName);
			string downloadDateString = fileName.Split('_').ElementAt(1) + fileName.Split('_').ElementAt(2);
			string mime = MimeMapping.GetMimeMapping(fileName);
			string filename = $"名称_{downloadDateString}.xlsx";

			return File(fileStream, mime, filename);
		}



		/// <summary>
		/// Excel头名称
		/// </summary>
		private static readonly string[] HeaderText =
		{
			"主体名称",
			"企业",
			"主体评级"
		};


		public class Product
		{
			public int FundId { get; set; }
			public int PvalueId { get; set; }
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



		/// <summary>
		/// 将数据源写入excel
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <param name="fileName">文件名称</param>
		/// <param name="headerText">列头</param>
		/// <param name="dataText">数据源</param>
		/// <param name="extraMessage"></param>
		/// <returns>回传excel路径名称</returns>
		public string ExportToFile(string filePath, string fileName,
			IEnumerable<string> headerText,
			IEnumerable<IEnumerable<string>> dataText,
			string extraMessage = null)
		{
			string fullFileName = GetFullFileName(filePath, fileName);
			IWorkbook workbook = Write(headerText, dataText, extraMessage);

			using (var fs = new FileStream(fullFileName, FileMode.Create))
			{
				workbook.Write(fs);
			}

			fileName = string.Concat(fileName, ".xlsx");
			return fileName;
		}

		private static string GetFullFileName(string filePath, string fileName)
		{
			if (string.IsNullOrEmpty(filePath) || fileName.Length <= 0)
			{
				Console.WriteLine(@"获取保存路径错误, filePath:{0}, fileName:{1}", filePath, fileName);
				return null;
			}

			if (!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}

			try
			{
				string checkedFileName = filePath + fileName + ".xlsx";

				if (File.Exists(checkedFileName))
				{
					int index = 1;

					do
					{
						checkedFileName = string.Format("{0}{1}({2}){3}",
							filePath, fileName, index, ".xlsx");
						index++;
					} while (File.Exists(checkedFileName));
				}

				return checkedFileName;
			}
			catch (Exception ex)
			{
				NLog.LogManager.GetCurrentClassLogger().Error(ex, "获取保存路径错误");
				throw;
			}
		}

		private static IWorkbook Write(
			IEnumerable<string> headerText,
			IEnumerable<IEnumerable<string>> dataText,
			string extraMessage = null)
		{
			IWorkbook wb = new XSSFWorkbook();
			ISheet sheet = wb.CreateSheet("Sheet1");
			var rowIndex = 0;

			if (!string.IsNullOrEmpty(extraMessage))
			{
				SetExcelRowForExtraMessage(sheet, extraMessage);
				rowIndex += 1;
			}

			SetExcelHeaderRow(sheet, rowIndex, headerText.ToList());
			rowIndex += 1;

			var dataList = dataText.Select(row => row as List<string> ?? row.ToList()).ToList();
			if (!dataList.Any())
			{
				return wb;
			}

			for (var i = 0; i < dataList.Count; i++)
			{
				SetExcelBodyRow(sheet, rowIndex + i, dataList.ElementAt(i));
			}

			//优化表格的列宽
			int maxColumnIndex = dataList.Max(row => row.Count);
			for (var i = 0; i < maxColumnIndex; i++)
			{
				sheet.AutoSizeColumn(i);
			}

			return wb;
		}

		private static void SetExcelRowForExtraMessage(ISheet sheet, string extraMessage)
		{
			sheet.CreateRow(0).CreateCell(0).SetCellValue(extraMessage);
		}

		private static void SetExcelHeaderRow(ISheet sheet, int rowIndex, IReadOnlyList<string> headerText)
		{
			if (headerText == null || !headerText.Any())
			{
				throw new ArgumentNullException("headerText");
			}

			sheet.CreateRow(rowIndex).CreateCell(0).SetCellValue(headerText[0]);

			for (int i = 1; i < headerText.Count; i++)
			{
				sheet.GetRow(rowIndex).CreateCell(i).SetCellValue(headerText[i]);
			}
		}

		private static void SetExcelBodyRow(ISheet sheet, int rowIndex, IReadOnlyList<string> dataText)
		{
			if (dataText == null || !dataText.Any())
			{
				throw new ArgumentNullException("dataText");
			}

			sheet.CreateRow(rowIndex).CreateCell(0).SetCellValue(dataText[0]);

			for (int i = 1; i < dataText.Count; i++)
			{
				sheet.GetRow(rowIndex).CreateCell(i).SetCellValue(dataText[i]);
			}
		}
	}
}