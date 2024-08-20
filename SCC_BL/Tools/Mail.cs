using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace SCC_BL.Tools
{
    public class Mail : IDisposable
    {
        public void SendMail(string emailAddress, string message, string subject, string filePath = "")
        {
            bool isTest = false;

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient sending = new System.Net.Mail.SmtpClient();

            if (isTest)
            {
                sending.Host = Settings.SMTP.Test.HOST;
                sending.Port = Settings.SMTP.Test.PORT;
                sending.EnableSsl = Settings.SMTP.Test.ENABLE_SSL;

                string systemEmailAddress = Settings.SMTP.Test.SYSTEM_EMAIL_ADDRESS;
                string systemEmailPassword = Settings.SMTP.Test.PASSWORD;

                try
                {
                    mail.To.Clear();
                    mail.Body = message;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.Subject = subject;
                    mail.IsBodyHtml = Settings.SMTP.Test.IS_BODY_HTML;
                    mail.To.Add(emailAddress.Trim());

                    if (!filePath.Equals(""))
                    {
                        System.Net.Mail.Attachment file = new System.Net.Mail.Attachment(filePath);
                        mail.Attachments.Add(file);
                    }

                    mail.From = new System.Net.Mail.MailAddress(systemEmailAddress);
                    sending.Credentials = new System.Net.NetworkCredential(systemEmailAddress, systemEmailPassword);

                    sending.Send(mail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sending.Dispose();
                }
            }
            else
            {
                sending.Host = Settings.SMTP.SCC.HOST;
                sending.Port = Settings.SMTP.SCC.PORT;
                sending.EnableSsl = Settings.SMTP.SCC.ENABLE_SSL;

                string systemEmailAddress = Settings.SMTP.SCC.SYSTEM_EMAIL_ADDRESS;
                string systemEmailPassword = Settings.SMTP.SCC.PASSWORD;

                try
                {
                    mail.To.Clear();
                    mail.Body = message;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.Subject = subject;
                    mail.IsBodyHtml = Settings.SMTP.SCC.IS_BODY_HTML;
                    mail.To.Add(emailAddress.Trim());


                    if (!filePath.Equals(""))
                    {
                        System.Net.Mail.Attachment file = new System.Net.Mail.Attachment(filePath);
                        mail.Attachments.Add(file);
                    }

                    mail.From = new System.Net.Mail.MailAddress(systemEmailAddress);
                    sending.Credentials = new System.Net.NetworkCredential(systemEmailAddress, systemEmailPassword);

                    sending.Send(mail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sending.Dispose();
                }
            }
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(
                    email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public void Dispose()
        {

        }
    }
}
