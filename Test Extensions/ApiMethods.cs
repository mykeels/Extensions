using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using Extensions;

namespace Test_Extensions
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
    }
}