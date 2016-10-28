using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Models
{
    public class IP2Loc
    {
        private string _ipaddress = "";

        public IP2Loc() { }

        public IP2Loc(string ipAddress)
        {
            this._ipaddress = ipAddress;
        }

        public string GetIpAddress()
        {
            if (String.IsNullOrEmpty(_ipaddress))
            {
                if (Site.Context() != null && Site.Context().Request != null)
                {
                    _ipaddress = System.Net.Dns.GetHostAddresses(Site.Context().Request.UserHostAddress).GetValue(0).ToString();
                }
            }
            return _ipaddress;
        }

        public IP2Loc.Location GetLocationIpGeni()
        {
            IP2Loc.Location ret = null;
            if (String.IsNullOrEmpty(_ipaddress)) _ipaddress = GetIpAddress();
            if (!String.IsNullOrEmpty(_ipaddress))
            {
                ret = Api.Get<IP2Loc.Location>("http://www.ipgeni.com/api/" + _ipaddress);
                if (ret != null)
                {
                    ret.ipaddress = _ipaddress;
                    if (!String.IsNullOrEmpty(ret.country)) return ret;
                    else return null;
                }
                else return null;
            }
            else return ret;
        }

        public IP2Loc.Location GetLocationDpIp()
        {
            IP2Loc.Location ret = null;
            string apiSecret = Site.AppSettings("DB-IP-API-KEY");
            if (String.IsNullOrEmpty(apiSecret)) apiSecret = "13d6fa4d0182803fe2fc481c032195a46325eeb6";
            if (String.IsNullOrEmpty(_ipaddress)) _ipaddress = GetIpAddress();
            if (!String.IsNullOrEmpty(_ipaddress))
            {
                ret = Api.Get<IP2Loc.Location>("http://api.db-ip.com/addrinfo?addr=" + _ipaddress + "&api_key=" + apiSecret);
                if (ret != null)
                {
                    ret.ipaddress = _ipaddress;
                    if (!String.IsNullOrEmpty(ret.country)) return ret;
                    else return null;
                }
                else return null;
            }
            else return ret;
        }

        public IP2Loc.Location GetLocationIpApi()
        {
            IP2Loc.Location ret = null;
            if (String.IsNullOrEmpty(_ipaddress)) _ipaddress = GetIpAddress();
            if (!String.IsNullOrEmpty(_ipaddress))
            {
                ret = Api.Get<IP2Loc.Location>("https://ipapi.co/" + _ipaddress + "/json");
                if (ret != null)
                {
                    ret.ipaddress = _ipaddress;
                    if (!String.IsNullOrEmpty(ret.country)) return ret;
                    else return null;
                }
                else return null;
            }
            else return ret;
        }

        public IP2Loc.Location GetLocationAll()
        {
            var loc = GetLocationIpGeni();
            if (loc != null) return loc.addToSession();
            else
            {
                loc = GetLocationDpIp();
                if (loc != null) return loc.addToSession();
                else
                {
                    loc = GetLocationIpApi();
                    if (loc != null) return loc.addToSession();
                    else
                    {
                        return new Location()
                        {
                            ip = "::1",
                            country = "US"
                        };
                    }
                }
            }
        }

        public static IP2Loc.Location GetSessionLocation()
        {
            IP2Loc loc = new IP2Loc();
            return loc.GetLocationAll();
        }

        public class Location
        {
            private string _cityName;
            private string _countryName;
            private string _stateRegion;
            private string _ipAddress;
            public string ip { get
                {
                    return _ipAddress;
                }
                set
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        _ipAddress = value;
                    }
                }
            }
            public string ipaddress
            {
                get
                {
                    return _ipAddress;
                }
                set
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        _ipAddress = value;
                    }
                }
            }
            public string city
            {
                get
                {
                    return _cityName;
                }
                set
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        _cityName = value;
                    }
                }
            }
            public string cityname
            {
                get
                {
                    return _cityName;
                }
                set
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        _cityName = value;
                    }
                }
            }
            public string region
            {
                get
                {
                    return _stateRegion;
                }
                set
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        _stateRegion = value;
                    }
                }
            }
            public string stateprovince
            {
                get
                {
                    return _stateRegion;
                }
                set
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        _stateRegion = value;
                    }
                }
            }
            public string country
            {
                get
                {
                    return _countryName;
                }
                set
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        _countryName = value;
                    }
                }
            }
            public string countrycode { get; set; }
            public string postal { get; set; }
            public string timezone { get; set; }
            public string phone { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }

            public Location addToSession()
            {
                if (Site.Context() != null && Site.Context().Session != null)
                {
                    Site.Context().Session.Add("ip_user", this);
                    Site.Context().Session.Add("ip_countrycode", this.country);
                    Site.Context().Session.Add("ip_cityname", this.city);
                    Site.Context().Session.Add("ip_stateprovince", this.stateprovince);
                    Site.Context().Session.Add("ip_address", this.ipaddress);
                    Site.Context().Session.AddSafe("ip_user_set", false);
                }
                return this;
            }
        }
    }
}
