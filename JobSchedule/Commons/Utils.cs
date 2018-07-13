using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Commons
{
	public static class Utils
	{
		/// <summary>
		/// 发邮件
		/// </summary>
		/// <param name="subject">主题</param>
		/// <param name="body">邮件内容</param>
		/// <param name="path">附件地址</param>
		public static void SendEmail(string subject, string body)
		{
			Task.Factory.StartNew(() =>
			{
				//发件人
				string mailFrom = ConfigurationManager.AppSettings["EmailSender"];
				//收件人(多个收件人时用分号';'或者','隔开)
				string mailTo = ConfigurationManager.AppSettings["DeveloperEmails"];
				//密码
				string senderCredentials = ConfigurationManager.AppSettings["Credentials"];
				var mailboxs = mailTo.Split(';', ',');
				var mailMessage = new MailMessage();
				foreach (string mailbox in mailboxs)
				{
					mailMessage.To.Add(mailbox);
				}
				mailMessage.From = new MailAddress(mailFrom);
				//mailMessage.CC.Add("cc@qq.com");//抄送人地址
				mailMessage.Subject = subject;//邮件标题 
				mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;//标题格式为UTF8
				mailMessage.Body = body;//邮件内容 
				mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//内容格式为UTF8

				var client = new SmtpClient
				{
					//Port= 1995,//端口号未知
					Host = "smtpcom.263xmail.com",
					//EnableSsl = true,//启用SSL加密
					Credentials = new NetworkCredential(mailFrom, senderCredentials)//账号，密码
				};

				client.Send(mailMessage);//发送邮件
			});
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
		public static string ExportToFile(string filePath, string fileName, IEnumerable<string> headerText, IEnumerable<IEnumerable<string>> dataText, string extraMessage = null)
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
				throw ex;
			}
		}

		private static IWorkbook Write(IEnumerable<string> headerText, IEnumerable<IEnumerable<string>> dataText, string extraMessage = null)
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
