using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace Extensions
{
    public class Api
    {
        public static string Get(string url)
        {
            WebClient client = new WebClient();
            client.BaseAddress = url;
            Stream stream = new MemoryStream();
            stream = client.OpenRead(url);
            string b = "";
            using (System.IO.StreamReader br = new System.IO.StreamReader(stream))
            {
                try
                {
                    b = br.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return b;
        }

        public static Promise GetAsync(string url)
        {
            //return Promise.Create(() => Get(url));
            return new Promise(() => Get(url));
        }

        public static Bitmap GetImage(string url)
        {
            WebClient client = new WebClient();
            client.BaseAddress = url;

            try
            {
                byte[] b = client.DownloadData(url);
                return b.ToBitmap();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Promise GetImageAsync(string url)
        {
            return new Promise(() => GetImage(url));
        }

        public static string Post(string url, string value, string contenttype = "text/xml", Dictionary<string, string> headers = null)
        {
            WebClient w = new WebClient();
            if ((headers != null))
            {
                foreach (var h_loopVariable in headers)
                {
                    var h = h_loopVariable;
                    w.Headers.Add(h.Key, h.Value);
                }
            }
            w.Headers.Add("Content-Type", contenttype);
            w.Headers.Add("Accept", "text/plain, " + contenttype);
            return w.UploadString(url, value);
        }

        public static Promise PostAsync(string url, string value, string contenttype = "text/xml", Dictionary<string, string> headers = null)
        {
            return new Promise(() => Post(url, value, contenttype, headers));
        }

        private static void browserWait(WebBrowser browser)
        {
            bool completed = false;
            ((WebBrowser)browser).DocumentCompleted += (sender, e) => completed = true;

            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            while (!completed) ;
        }

        /// <summary>
        /// This returns a promise whose return type is a Web Browser. Use the browser as you wish.
        /// browser.DocumentCompleted delegate might help you here.
        /// browser.DocumentText will give you the executed html text
        /// </summary>
        /// <param name="path"></param>
        /// <param name="browserwidth"></param>
        /// <param name="browserheight"></param>
        /// <returns>A Promise to return a WebBrowser object</returns>
        public static Promise Execute(string path, BrowserType type = BrowserType.WebBrowser, int browserwidth = 1024, int browserheight = 768)
        {
            return Promise.Create(() =>
            {
                WebBrowser browser;
                if (type == BrowserType.MobileWebBrowser) browser = new MobileWebBrowser();
                else browser = new WebBrowser();
                browser.ScrollBarsEnabled = false;
                browser.AllowNavigation = true;
                browser.Width = browserwidth;
                browser.Height = browserheight;
                browser.ScriptErrorsSuppressed = true;
                browser.Url = new Uri(path);
                browser.DocumentText = Api.Get(path);
                browserWait(browser);
                return browser;
            });
        }

        public enum BrowserType
        {
            WebBrowser,
            MobileWebBrowser
        }

        public static List<string> GetGoogleImages(string query)
        {
            string path = "https://www.google.com.ng/search?q=" + query + "&tbm=isch";
            List<string> ret = new List<string>();
            string regtext = "src=\"(.+?)\"";
            Regex regex = new Regex(regtext);
            Execute(path).Success((browser) =>
            {
                string doctext = ((WebBrowser)browser).DocumentText;
                if (!string.IsNullOrEmpty(doctext))
                {
                    MatchCollection matches = regex.Matches(doctext);
                    for (int i = 0; i <= Math.Min(matches.Count - 1, 30); i++)
                    {
                        Match match = matches[i];
                        ret.Add(match.Value.Replace("src=", "").Replace("\"", "").Split(new string[] { "&amp" }, StringSplitOptions.None)[0]);
                    }
                }
            }).current.Join();
            return ret;
        }
    }
}