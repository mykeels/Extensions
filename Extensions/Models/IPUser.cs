using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Extensions.Models
{
    public class IPUser
    {
        public Location.Country country { get; set; }
        public Location.City city { get; set; }
        public string countrycode { get; set; }
        public string cityname { get; set; }
        public string stateprovince { get; set; }
        public string ipaddress { get; set; }

        public IPUser()
        {
            this.country = new Location.Country();
            this.city = new Location.City();
        }

        public static dynamic CreateNewUser()
        {
            string ipaddress = GetIpAddress();
            dynamic userlocation = new
            {
                ip = "",
                hostname = false,
                city = "",
                region = "",
                country = "",
                loc = "",
                org = "",
                postal = new { },
                phone = "",
                stateprov = ""
            };
            try
            {
                userlocation = GetUserLocation(ipaddress);
            }
            catch (Exception ex) { }
            EnsureUserLocation(ipaddress, ref userlocation);
            if (Site.Context().Session["ip_user"] == null)
            {
                var location = Api.Get<IpApiLocation>("https://ipapi.co/" + ipaddress + "/json/");
                Site.Context().Session.Add("ip_user", location);
                Site.Context().Session.AddSafe("ip_user_set", false);
            }
            if (Site.Context() != null && Site.Context().Session != null)
            {
                Site.Context().Session.AddSafe("site-default-country", Site.GetSiteCountryCode());
                Site.Context().Session.AddSafe("site-default-currency", Currency.GetSiteCurrencyCode());
            }
            return userlocation;
        }

        public static string GetIpAddress()
        {
            return System.Net.Dns.GetHostAddresses(Site.Context().Request.UserHostAddress).GetValue(0).ToString(); // "104.194.196.92"
        }

        public static dynamic GetUserLocation(string ipaddress)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(Api.Get("http://www.ipgeni.com/api/" + ipaddress));
        }

        public static void EnsureUserLocation(string ipaddress, ref dynamic userlocation)
        {
            try
            {
                if (userlocation == null)
                {
                    userlocation = new
                    {
                        ip = "",
                        hostname = false,
                        city = "",
                        region = "",
                        stateprov = "",
                        country = "",
                        loc = "",
                        org = "",
                        postal = new { },
                        phone = ""
                    };
                }
                bool boolres = true;
                var hostname = userlocation.hostname;
                try
                {
                    if (hostname == false || Boolean.TryParse((string)hostname.Value, out boolres) == false)
                    {
                        userlocation = JsonConvert.DeserializeObject(Api.Get("http://api.db-ip.com/addrinfo?addr=" + ipaddress + "&api_key=13d6fa4d0182803fe2fc481c032195a46325eeb6"));
                        try
                        {
                            if (userlocation.error == null)
                            {
                                AddUserToSession(ipaddress, ref userlocation);
                            }
                        }
                        catch (Exception ex)
                        {
                            AddUserToSession(ipaddress, ref userlocation);
                        }
                    }
                    else
                    {
                        AddUserToSession(ipaddress, ref userlocation);
                    }
                }
                catch (Exception ex)
                {
                    AddUserToSession(ipaddress, ref userlocation);
                }

            }
            catch (Exception ex)
            {
                Models.IPUser user = new Models.IPUser();
                user.countrycode = userlocation.country;
                if (!String.IsNullOrEmpty((string)(userlocation.stateprov)))
                {
                    user.stateprovince = userlocation.stateprov;
                }
                user.ipaddress = ipaddress;
                user.country = Location.Country.GetCountryByCode(user.countrycode);
                if (Site.Context() != null && Site.Context().Session != null)
                {
                    Site.Context().Session.Add("ip_user", user);
                    Site.Context().Session.AddSafe("ip_user_set", false);
                }
                //Site.LogError(ex);
            }
        }

        public static void AddUserToSession(string ipaddress, ref dynamic userlocation)
        {
            try
            {
                if (userlocation == null)
                {
                    userlocation = new
                    {
                        ip = "",
                        hostname = false,
                        city = "",
                        region = "",
                        stateprov = "",
                        stateprovince = "",
                        country = "",
                        loc = "",
                        org = "",
                        postal = new { },
                        phone = ""
                    };
                }
                userlocation = ((JObject)userlocation);
                Models.IPUser user = new Models.IPUser();
                user.countrycode = userlocation.GetValue("country");
                user.cityname = userlocation.GetValue("city");
                user.stateprovince = userlocation.GetValue("region");
                if (!String.IsNullOrEmpty((string)((JObject)userlocation).GetValue("stateprov")))
                {
                    user.stateprovince = userlocation.GetValue("stateprov");
                }
                user.ipaddress = ipaddress;
                user.country = Location.Country.GetCountryByCode(user.countrycode);
                user.city = Location.City.GetCity(user.country.Name, user.stateprovince);
                Site.Context().Session.AddSafe("ip_user", user);
                Site.Context().Session.AddSafe("ip_user_set", false);
            }
            catch (Exception ex)
            {
                userlocation = ((JObject)userlocation);
                Models.IPUser user = new Models.IPUser();
                user.countrycode = userlocation.GetValue("country");
                if (!String.IsNullOrEmpty((string)((JObject)userlocation).GetValue("stateprov")))
                {
                    user.stateprovince = userlocation.GetValue("stateprov");
                }
                user.ipaddress = ipaddress;
                user.country = Location.Country.GetCountryByCode(user.countrycode);
                Site.Context().Session.Add("ip_user", user);
                Site.Context().Session.AddSafe("ip_user_set", false);
                //Site.LogError(ex);
            }

        }

        public class IpApiLocation
        {
            public string ip { get; set; }
            public string city { get; set; }
            public string region { get; set; }
            public string country { get; set; }
            public string postal { get; set; }
            public string timezone { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
        }
    }
}