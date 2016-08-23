using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax;

namespace CdnBundle
{
    public static class BundleListExtensions
    {
        private static Dictionary<string, DateTime> cacheRecords = new Dictionary<string, DateTime>();
        public static string Load(this IEnumerable<Bundle> bundles, string localUrl = null, bool async = false)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Bundle bundle in bundles)
            {
                sb.Append(bundle.Load());
            }
            string response = sb.ToString();
            if (!String.IsNullOrEmpty(localUrl))
            {
                if (!cacheRecords.ContainsKey(localUrl) || (cacheRecords.ContainsKey(localUrl) && (DateTime.Now.Subtract(cacheRecords[localUrl]).TotalHours > 24)))
                {
                    cacheRecords.Add(localUrl, DateTime.Now);
                    response.SaveToFile(Bundle.getLocalFilePath(localUrl));
                }
                if (bundles.All((b) => b.type == Bundle.BundleType.CSS))
                {
                    // css link stylesheet
                    return "<link href=\"" + Bundle.getResolvePath(localUrl) + "\" type=\"text/css\"" + (async ? " async" : "") + " rel =\"stylesheet\"></link>";
                }
                else
                {
                    //js script tag
                    return "<script src=\"" + Bundle.getResolvePath(localUrl) + "\"" + (async ? " async" : "") + "></script>";
                }
            }
            else
            {
                if (bundles.All((b) => b.type == Bundle.BundleType.CSS)) return "<style>" + response + "</style";
                else return "<script>" + response + "</script>";
            }
        }
    }
    public class Bundle
    {
        public string cdnUrl { get; set; }
        public string localUrl { get; set; }
        public bool useMinification { get; set; }
        public BundleType type { get; set; }
        private static Dictionary<string, DateTime> cacheRecords = new Dictionary<string, DateTime>();

        public Bundle()
        {

        }

        public Bundle(string cdnUrl, string localUrl, BundleType bundleType, bool useMinification = true)
        {
            this.cdnUrl = cdnUrl;
            this.localUrl = localUrl;
            this.type = bundleType;
            this.useMinification = useMinification;
        }

        public enum BundleType
        {
            JavaScript,
            CSS
        }

        private static string GetLeftUrl()
        {
            string lefturl = "";
            try
            {
                if (!String.IsNullOrEmpty(lefturl)) return lefturl;
                string left = String.Format("{0}://{1}{2}/", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Authority, HttpRuntime.AppDomainAppVirtualPath);
                while (left.EndsWith("/"))
                {
                    left = left.Substring(0, left.Length - 1);
                }
                lefturl = left.ToLower() + "/";
                return lefturl;
            }
            catch (Exception ex)
            {
                return "/";
            }
        }

        public string getLocalFilePath()
        {
            return getLocalFilePath(localUrl);
        }

        public static string getLocalFilePath(string localUrl)
        {
            string localUrlPath = localUrl;
            if (HttpContext.Current != null && HttpContext.Current.Server != null && localUrl.StartsWith("~"))
            {
                localUrlPath = HttpContext.Current.Server.MapPath(localUrl);
            }
            return localUrlPath;
        }

        public static string getResolvePath(string localUrl)
        {
            string localUrlPath = localUrl;
            if (HttpContext.Current != null && HttpContext.Current.Server != null && localUrl.StartsWith("~"))
            {
                localUrlPath = localUrl.Replace("~/", GetLeftUrl());
            }
            return localUrlPath;
        }

        public string Load()
        {
            StringBuilder sb = new StringBuilder();
            var minifier = new Microsoft.Ajax.Utilities.Minifier();
            if (!String.IsNullOrEmpty(cdnUrl) && (!cacheRecords.ContainsKey(cdnUrl) || DateTime.Now.Subtract(cacheRecords[cdnUrl]).TotalHours > 24))
            {
                string response = "";
                try
                {
                    if (cdnUrl.StartsWith("~/")) cdnUrl = cdnUrl.Replace("~/", GetLeftUrl());
                    response = Api.Get(cdnUrl);
                    if (!cacheRecords.ContainsKey(cdnUrl)) cacheRecords.Add(cdnUrl, DateTime.Now);
                }
                catch (Exception ex)
                {
                    if (!String.IsNullOrEmpty(localUrl)) response = System.IO.File.ReadAllText(getLocalFilePath());
                    else throw ex;
                }
                if (useMinification)
                {
                    if (type == BundleType.CSS) response = minifier.MinifyStyleSheet(response);
                    else response = minifier.MinifyJavaScript(response);
                }
                if (!String.IsNullOrEmpty(localUrl))
                {
                    response.SaveToFile(getLocalFilePath());
                }
                sb.Append(response);
                sb.AppendLine();
            }
            else
            {
                if (System.IO.File.Exists(getLocalFilePath()))
                {
                    sb.Append(System.IO.File.ReadAllText(getLocalFilePath()));
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        public Promise<string> LoadAsync()
        {
            return Promise<string>.Create(() => Load());
        }
    }
}