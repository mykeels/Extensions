using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Mail;
using SendGrid;
using System.IO;
using System.Diagnostics;

namespace Extensions.Models
{
    public class Mail
    {
        public static bool Send(string EmailTo, string Subject, string Body, string fromaddress = null, string displayname = "Wakanow", List<string> CCs = null)
        {
            if (!String.IsNullOrEmpty(Site.AppSettings("MailCloudApiKey")))
            {
                SendViaCloud(EmailTo, Subject, Body, fromaddress, displayname, CCs).Wait();
                return true;
            }
            MailAddress fromAddress = null;
            if (String.IsNullOrEmpty(fromaddress)) fromAddress = new MailAddress(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EmailFromAddress"]), displayname);
            else fromAddress = new MailAddress(fromaddress, displayname);
            using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
            {
                using (MailMessage usrMail = new MailMessage())
                {
                    usrMail.From = fromAddress;
                    usrMail.Body = Body;
                    usrMail.Subject = Subject;
                    usrMail.IsBodyHtml = true;
                    usrMail.To.Add(EmailTo);
                    if (CCs != null)
                    {
                        CCs.ForEach((cc) =>
                        {
                            usrMail.CC.Add(cc);
                        });
                    }
                    smtp.Send(usrMail);
                    return true;
                }
            }
        }

        public static Task SendViaCloud(string EmailTo, string Subject, string Body, string fromaddress = null, string displayname = "Wakanow", List<string> CCs = null, Dictionary<string, Stream> attachments = null)
        {
            MailAddress fromAddress = null;
            if (String.IsNullOrEmpty(fromaddress)) fromAddress = new MailAddress(Site.AppSettings("EmailFromAddress"), displayname);
            else fromAddress = new MailAddress(fromaddress, displayname);
            var usrMail = new SendGridMessage();
            usrMail.From = fromAddress;
            usrMail.Html = Body;
            usrMail.Subject = Subject;
            usrMail.AddTo(EmailTo);
            if (CCs != null)
            {
                CCs.ForEach((cc) =>
                {
                    usrMail.AddCc(cc);
                });
            }
            if (attachments != null)
            {
                foreach (string key in attachments.Keys)
                {
                    usrMail.AddAttachment(attachments[key], key);
                }
            }
            var transportWeb = new Web(Site.AppSettings("MailCloudApiKey"));
            return transportWeb.DeliverAsync(usrMail);
        }

        public static Promise SendAsync(string EmailTo, string Subject, string Body, string fromaddress = null, string displayname = "Wakanow", List<string> CCs = null)
        {
            return new Promise(() =>
            {
                return Send(EmailTo, Subject, Body, fromaddress, displayname, CCs);
            });
        }

        public static string GetMailBody(string path, Dictionary<string, dynamic> replaces = null)
        {
            string body = "";
            if (path.Contains("~/"))
            {
                body = System.IO.File.ReadAllText(Site.Context().Server.MapPath(path));
            }
            else
            {
                path = path.Replace("~/", Site.GetLeftUrl());
                body = Api.Get(path);
            }
            if (replaces != null)
            {
                foreach (var pair in replaces)
                {
                    body = body.Replace(pair.Key, pair.Value);
                }
            }
            return body;
        }

        public static string GetMailBody(string path, params KeyValue[] keyvalues)
        {
            string body = "";
            if (path.Contains("~/"))
            {
                body = System.IO.File.ReadAllText(Site.Context().Server.MapPath(path));
            }
            else
            {
                path = path.Replace("~/", Site.GetLeftUrl());
                body = Api.Get(path);
            }
            body = body.Replace("~/", Site.GetLeftUrl());
            foreach (var pair in keyvalues)
            {
                body = body.Replace(pair.key, pair.value);
            }
            return body;
        }

    }
}
