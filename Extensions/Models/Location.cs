using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Models
{
    public class Location
    {
        public static List<Continent_Country> Continent_Country_Pairs { get; set; }

        public static City FixCityAndCountry(ref string countryname, ref string cityname)
        {
            Location.City thiscity = Location.City.GetCity(countryname, cityname);
            if (thiscity == null) return null;
            countryname = thiscity.CountryName;
            cityname = thiscity.Name;
            return thiscity;
        }

        public class Continent_Country
        {
            public string ContinentCode { get; set; }
            public string CountryCode { get; set; }

            public static List<Continent_Country> GetContinentCountryPairs()
            {
                if (Continent_Country_Pairs == null)
                {
                    Continent_Country_Pairs = new List<Continent_Country>();
                    foreach (string s in System.Text.Encoding.UTF8.GetString(Properties.Resources.continents).Split('\n'))
                    {
                        Continent_Country cc = new Continent_Country();
                        var items = s.Split(',');
                        cc.CountryCode = items[0];
                        cc.ContinentCode = items[1];
                        Continent_Country_Pairs.Add(cc);
                    }
                    return Continent_Country_Pairs;
                }
                return Continent_Country_Pairs;
            }
        }

        public class Continent
        {
            public string Code { get; set; }
            public string Name { get; set; }

            public Continent()
            {

            }

            public Continent(string code = "", string name = "")
            {
                this.Code = code;
                this.Name = name;
            }

            public static Continent GetContinent(string code)
            {
                return (from pp in GetAllContinents() where pp.Code.ToLower().Trim().Equals(code.ToLower().Trim()) select pp).FirstOrDefault(new Continent("--"));
            }

            public static Continent GetContinentByCountryCode(string code)
            {
                return GetContinent((from pp in Location.Continent_Country.GetContinentCountryPairs() where pp.CountryCode.ToLower().Trim().Equals(code.ToLower().Trim()) select pp).FirstOrDefault(new Continent_Country()).ContinentCode);
            }

            public List<string> GetCountries()
            {
                return (from pp in Location.Continent_Country.GetContinentCountryPairs() where pp.ContinentCode.Equals(this.Code) select pp.CountryCode).ToList();
            }

            public static List<Continent> GetAllContinents()
            {
                List<Continent> ret = new List<Continent>();
                ret.Add(new Continent("EU", "Europe"));
                ret.Add(new Continent("AS", "Asia"));
                ret.Add(new Continent("NA", "North America"));
                ret.Add(new Continent("AF", "Africa"));
                ret.Add(new Continent("SA", "South America"));
                ret.Add(new Continent("OC", "Pceania"));
                ret.Add(new Continent("AN", "Antarctica"));
                return ret;
            }
        }

        public class Country
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Continent { get; set; }
            public string ContinentCode { get; set; }

            public static List<Country> Countries { get; set; }

            public static List<Country> GetCountries()
            {
                if (Countries == null)
                {
                    byte[] countries_bytes = Properties.Resources.countries;
                    string countries_text = Encoding.UTF8.GetString(countries_bytes);
                    Countries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Country>>(countries_text);
                    return Countries;
                }
                else
                {
                    return Countries;
                }
            }

            public static Country GetCountry(string countryname)
            {
                if (Countries == null) GetCountries();
                return (from pp in Countries where pp.Name.EncodeURI().Equals(countryname.EncodeURI()) select pp).FirstOrDefault();
            }

            public static Country GetCountryByCode(string countrycode)
            {
                if (Countries == null) GetCountries();
                if (countrycode.ToLower() == "GB") countrycode = "UK";
                return (from pp in Countries where pp.Code.ToLower().Equals(countrycode.ToLower()) select pp).FirstOrDefault();
            }

            public static Country GetCountryByName(string countryname)
            {
                if (Countries == null) GetCountries();
                return (from pp in Countries where pp.Name.ToLower().Equals(countryname.ToLower()) select pp).FirstOrDefault();
            }

            public static string GetCountryName(string countryname)
            {
                Country country = GetCountry(countryname);
                if (country == null) return "";
                return country.Name;
            }

            public static string GetCountryCode(string countryname)
            {
                Country country = GetCountry(countryname);
                if (country == null) return "";
                return country.Code;
            }
        }

        public class City
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string CountryCode { get; set; }
            public string CountryName { get; set; }
            public static List<City> Cities { get; set; }

            public static List<City> GetCities()
            {
                if (Cities == null)
                {
                    byte[] cities_bytes = Properties.Resources.cities;
                    string cities_text = Encoding.UTF8.GetString(cities_bytes);
                    Cities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<City>>(cities_text);
                    return Cities;
                }
                else
                {
                    return Cities;
                }
            }

            public static City GetCity(string countryname, string cityname)
            {
                if (Cities == null) GetCities();
                return (from pp in Cities where pp.CountryName.EncodeURI().Equals(countryname.EncodeURI()) && pp.Name.EncodeURI().Equals(cityname.EncodeURI()) select pp).FirstOrDefault();
            }

            public static string GetCityName(string countryname, string cityname)
            {
                City city = GetCity(countryname, cityname);
                if (city == null) return "";
                return city.Name;
            }

            public static string GetCityCode(string countryname, string cityname)
            {
                City city = GetCity(countryname, cityname);
                if (city == null) return "";
                return city.Code;
            }
        }
    }
}
