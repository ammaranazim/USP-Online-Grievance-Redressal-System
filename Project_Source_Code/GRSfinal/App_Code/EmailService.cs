
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

public static class EmailService
{
    public static void SendEmail(
        string recipient,
        string subject,
        string body)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient email is required.");

        string host = ConfigurationManager.AppSettings["SmtpHost"];
        string portValue = ConfigurationManager.AppSettings["SmtpPort"];
        string senderEmail = ConfigurationManager.AppSettings["SmtpEmail"];
        string appPassword = ConfigurationManager.AppSettings["SmtpAppPassword"];
        string sslValue = ConfigurationManager.AppSettings["SmtpEnableSsl"];

        int port;
        bool enableSsl;

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(senderEmail) ||
            string.IsNullOrWhiteSpace(appPassword) ||
            !int.TryParse(portValue, out port) ||
            !bool.TryParse(sslValue, out enableSsl))
        {
            throw new ConfigurationErrorsException(
                "SMTP settings are missing or invalid in Web.config.");
        }

        using (MailMessage message = new MailMessage())
        {
            message.From = new MailAddress(senderEmail);
            message.To.Add(new MailAddress(recipient));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient(host, port))
            {
                client.EnableSsl = enableSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(
                    senderEmail, appPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Timeout = 30000;

                client.Send(message);
            }
        }
    }
}
