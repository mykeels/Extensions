using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Extensions;

namespace Extensions.Models
{
    public class Currency
    {
        public static List<Currency> Currencies = new List<Currency>();
        public static string Base_Code = "";
        public static string Default_Code = "";
        public static string Server_Code = "";

        public string Code { get; set; }
        public string Icon { get; set; }
        public bool Base { get; set; }
        public double Rate { get; set; }
        public string Name { get; set; }

        public Currency()
        {
            Code = "";
            Icon = "";
            Base = false;
            Rate = 0.00;
            Name = "";
        }

        public Currency(string code, string icon, bool base_, double rate, string name)
        {
            Code = code;
            Icon = icon;
            Base = base_;
            Rate = rate;
            Name = name;
        }

        public static string GetIcon(string code)
        {
            for (int i = 0; i < Currencies.Count - 1; i++)
            {
                Currency c = Currencies[i];
                if (c.Code == code)
                {
                    return c.Icon;
                }
            }
            return null;
        }

        public static Currency GetBaseCurrency()
        {
            for (int i = 0; i < Currencies.Count - 1; i++)
            {
                Currency c = Currencies[i];
                if (c.Base == true)
                {
                    Base_Code = c.Code;
                    return c;
                }
            }
            return null;
        }

        public static Currency SetBaseCurrency(string code)
        {
            if (GetCurrency(code) != null)
            {
                Currencies.ForEach((curr) =>
                {
                    curr.Base = false;
                });
                Currency currency = Currencies.Where((curr) => curr.Code == code).FirstOrDefault();
                currency.Base = true;
                Base_Code = code;
                return currency;
            }
            return null;
        }

        public static Currency GetDefaultCurrency()
        {
            for (int i = 0; i < Currencies.Count - 1; i++)
            {
                Currency c = Currencies[i];
                if (c.Code == Default_Code)
                {
                    return c;
                }
            }
            return null;
        }

        public static string GetSiteCurrencyCode()
        {
            Site.Type type = Site.GetSiteType();
            switch (type)
            {
                case Site.Type.Ghana: return "GHS";
                case Site.Type.UAE: return "AED";
                case Site.Type.UnitedKingdom: return "GBP";
                case Site.Type.USA: return "USD";
                case Site.Type.Nigeria: return "NGN";
                default: return "USD";
            }
        }

        public static Currency GetCurrencyByIcon(string icon)
        {
            for (int i = 0; i < Currencies.Count - 1; i++)
            {
                Currency c = Currencies[i];
                if (c.Icon == icon)
                {
                    return c;
                }
            }
            return null;
        }

        public static Currency GetCurrency(string code)
        {
            for (int i = 0; i <= Currencies.Count - 1; i++)
            {
                Currency c = Currencies[i];
                if (c.Code == code)
                {
                    return c;
                }
            }
            return new Currency();
        }

        public static void SetRate(string code, double rate)
        {
            GetCurrency(code).Rate = rate;
        }

        public static double GetRate(string code)
        {
            return GetCurrency(code).Rate;
        }

        public static double ConvertFromBase(double base_amt, string code)
        {
            if (code == Base_Code) return base_amt;
            return Math.Round(base_amt * GetRate(code), 2);
        }

        public static double ConvertToBase(double amount, string code)
        {
            if (code == Base_Code) return amount;
            return Math.Round(amount / GetRate(code), 2);
        }

        public static double ConvertBtwCurrencies(double amount1, string code1, string code2)
        {
            double amt_in_base = ConvertToBase(amount1, code1);
            return ConvertFromBase(amt_in_base, code2);
        }

        public static string Display(double amount, string code)
        {
            return GetIcon(code) + " " + Math.Round(amount, 2);
        }

        public static double Convert(double amount)
        {
            return ConvertBtwCurrencies(amount, Base_Code, Default_Code);
        }

        public static string ConvertD(double amount)
        {
            return Display(Convert(amount), GetDefaultCurrency().Icon);
        }

        public static void Init()
        {
            Currencies.PushIf(new Currency("NGN", "\u20a6", true, 199.20, "Nigerian Naira"), "Code");
            Currencies.PushIf(new Currency("USD", "\u0024", false, 1, "US Dollar"), "Code");
            Currencies.PushIf(new Currency("AED", "\u062f\u002e\u0625", false, 0.27, "Arab Emirate Dirham"), "Code");
            Currencies.PushIf(new Currency("GHS", "\u0047\u0048\u20b5", false, 0.26, "Ghanaian Cedi"), "Code");
            Currencies.PushIf(new Currency("GBP", "\u00a3", false, 1.54, "Great British Pound"), "Code");
            Currencies.PushIf(new Currency("EUR", "€", false, 1.34, "Euro"), "Code");
            Base_Code = Currencies.Where((curr) => curr.Base).FirstOrDefault().Code;
        }

        public static void Refresh()
        {
            Init();
            string path = "http://beta.wakanow.com/activities/CurrencyService.aspx?method=GetExchangeRates";
            var str = "";
            str = Currencies.Select("Code").Join(",");
            string base_currency_code = (string)Currencies.Where("Base", true).Select("Code").FirstOrDefault("NGN");
            string results = Api.Get(path + "&from=" + base_currency_code + "&to=" + str);
            JArray arr = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(results);
            foreach (var obj in arr)
            {
                string code = (string)obj["code"];
                double rate = (double)obj["amount"];
                var currencies = (from pp in Currencies where pp.Code.Equals(code) select pp);
                if (currencies.Count() > 0)
                {
                    currencies.First().Rate = rate;
                }
            }
        }

        public static Promise<bool> RefreshAsync()
        {
            return Promise<bool>.Create(() =>
            {
                Refresh();
                return true;
            });
        }

    }
}
