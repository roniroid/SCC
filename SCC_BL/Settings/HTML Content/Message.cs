using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Settings.HTML_Content
{
    public static class Message
	{
		public static string GetBody(Notification.Type type, string content)
		{
			string messageBody =
				@"<div class=""{0} "" role=""alert"">
					<span type = ""button"" class=""close"" data-bs-dismiss=""alert"" aria-label=""Close""><span aria-hidden=""true"">×</span>
					</span>
					<strong>{1}</strong>
				</div>";

			try
			{
                switch (type)
                {
                    case Notification.Type.SUCCESS:
						messageBody = messageBody.Replace("{0}", "alert alert-success alert-dismissible");
						break;
                    case Notification.Type.INFO:
						messageBody = messageBody.Replace("{0}", "alert alert-info alert-dismissible");
						break;
                    case Notification.Type.WARNING:
						messageBody = messageBody.Replace("{0}", "alert alert-warning alert-dismissible");
						break;
                    case Notification.Type.ERROR:
						messageBody = messageBody.Replace("{0}", "alert alert-danger alert-dismissible");
						break;
                    default:
                        break;
                }

				messageBody = messageBody.Replace("{1}", content);

				return messageBody;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
	}
}
