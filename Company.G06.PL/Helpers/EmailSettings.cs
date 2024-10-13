using Company.G06.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.G06.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			// Mail Server  : gmail.com
			// Smtp
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("eslamalaa2810@gmail.com", "ybeqkphxyefxcmgr");

           
            // ybeqkphxyefxcmgr

            client.Send("eslamalaa2810@gmail.com", email.To, email.Subject, email.Body);

		}
	}
}
