using System.Configuration;
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
	}
}
