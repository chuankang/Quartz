using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

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
		public static void SendEmail(string subject, string body, string path = null)
		{
			//发邮件
			var message = new MailMessage
			{
				From = new MailAddress("SkyE_WarningMai@kaytune.com")
			};

			message.To.Add("1037134@qq.com");
			message.Subject = subject;
			message.Body = body;

			#region 附件
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				var myAttachment = new Attachment(path, MediaTypeNames.Application.Octet)
				{
					NameEncoding = System.Text.Encoding.UTF8
				};
				message.Attachments.Add(myAttachment);
			}

			#endregion

			message.IsBodyHtml = true;
			var smtp = new SmtpClient("smtp.qiye.163.com")
			{
				UseDefaultCredentials = true,
				Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailName"],
					ConfigurationManager.AppSettings["EmailPwd"])
			};

			smtp.Send(message);
		}
	}
}
