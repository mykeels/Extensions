using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using Elmah;

namespace Extensions.Models
{
    public class Site
    {
        public static Regex MobileCheck = new Regex(@"android|(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        public static Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        public static string[] MobileDevices = new string[] {"iphone","ppc",
                                                   "windows ce","blackberry",
                                                   "opera mini","mobile","palm",
                                                   "portable","opera mobi" };

        public static string lefturl = GetLeftUrl();
        public static string GetLeftUrl()
        {
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
        
        public static string ResolveURL(string path)
        {
            path.TrimStart('/');
            if (path.Contains("~/")) path = path.Replace("~/", GetLeftUrl());
            else path = GetLeftUrl() + path;
            return path;
        }

        public static string MapPath(string path = "")
        {
            if (!path.StartsWith("~/")) return path;
            if (Context() == null) return path.Replace("~", System.Environment.CurrentDirectory);
            return Context().Server.MapPath(path);
        }

        public static HttpContext Context()
        {
            return System.Web.HttpContext.Current;
        }

        public static string AppSettings(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            if (val == null) val = "";
            return Convert.ToString(val);
        }

        public static void LogError(Exception ex)
        {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }

        public static bool IsMobile()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                var useragent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();
                if (useragent.Length < 4) return false;
                if (MobileCheck.IsMatch(useragent) || MobileVersionCheck.IsMatch(useragent.Substring(0, 4))) return true;
            }
            else
            {
                string useragent = HttpContext.Current.Request.UserAgent.ToString();
                if (MobileDevices.Any(x => useragent.Contains(x))) return true;
                else return Context().Request.Browser.IsMobileDevice;
            }
            return false;
        }

        public static Type GetSiteType()
        {
            string site_culture = Site.AppSettings("OverrideCulture").ToLower();
            var request = System.Web.HttpContext.Current.Request;
            if (request.Url.ToString().ToLower().Contains("/en-uk/") || site_culture.Equals("en-uk")) return Type.UnitedKingdom;
            else if (request.Url.ToString().ToLower().Contains("/en-us/") || site_culture.Equals("en-us")) return Type.USA;
            else if (request.Url.ToString().ToLower().Contains("/en-ae/") || site_culture.Equals("en-ae")) return Type.UAE;
            else if (request.Url.ToString().ToLower().Contains("/en-gh/") || site_culture.Equals("en-gh")) return Type.Ghana;
            else if (request.Url.ToString().ToLower().Contains("/en-ng/") || site_culture.Equals("en-ng")) return Type.Nigeria;
            else return Type.Nigeria;
        }

        public static string GetSiteUrl()
        {
            var request = System.Web.HttpContext.Current.Request;
            Type siteType = GetSiteType();
            if (request.Url.ToString().ToLower().Contains("localhost")) return "/";
            else if (request.Url.ToString().ToLower().Contains("beta")) return "/";
            else
            {
                if (siteType == Type.Ghana) return "/en-gh/";
                else if (siteType == Type.Nigeria) return "/en-ng/";
                else if (siteType == Type.USA) return "/en-us/";
                else if (siteType == Type.UnitedKingdom) return "/en-uk/";
                else if (siteType == Type.UAE) return "/en-ae/";
                else return "/";
            }
        }

        public static string GetSiteCountryCode()
        {
            Type type = GetSiteType();
            if (type == Type.Ghana) return "GH";
            if (type == Type.Nigeria) return "NG";
            if (type == Type.UnitedKingdom) return "GB";
            if (type == Type.UAE) return "AE";
            if (type == Type.USA) return "US";
            else return "NG";
        }

        public enum Type
        {
            Nigeria,
            UnitedKingdom,
            USA,
            Ghana,
            UAE
        }
    }
}