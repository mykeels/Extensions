using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Extensions;

namespace Extensions.Models
{
    public partial class Currency
    {
        public static List<Currency> Currencies = new List<Currency>();
        public static string Base_Code = "";
        public static string Default_Code = "";
        public static string Server_Code = "";

        public string Code { get; set; }
        public string Icon { get; set; }
        public bool Base { get; set; }
        public decimal Rate { get; set; }
        public string Name { get; set; }

        public Currency()
        {
            Code = "";
            Icon = "";
            Base = false;
            Rate = 0.00M;
            Name = "";
        }

        public Currency(string code, string icon, bool base_, decimal rate, string name)
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
                Currencies.ForEach((curr) => curr.Base = false);
                Currency currency = Currencies.Where((curr) => curr.Code == code).FirstOrDefault();
                currency.Base = true;
                Base_Code = code;
                Refresh();
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

        public static void SetRate(string code, decimal rate)
        {
            GetCurrency(code).Rate = rate;
        }

        public static decimal GetRate(string code)
        {
            return GetCurrency(code).Rate;
        }

        public static decimal ConvertFromBase(decimal base_amt, string code)
        {
            if (code == Base_Code) return base_amt;
            return Math.Round(base_amt * GetRate(code), 2);
        }

        public static decimal ConvertToBase(decimal amount, string code)
        {
            if (code == Base_Code) return amount;
            return Math.Round(amount / GetRate(code), 2);
        }

        public static decimal ConvertBtwCurrencies(decimal amount1, string code1, string code2)
        {
            decimal amt_in_base = ConvertToBase(amount1, code1);
            return ConvertFromBase(amt_in_base, code2);
        }

        public static string Display(decimal amount, string code)
        {
            return GetIcon(code) + " " + Math.Round(amount, 2);
        }

        public static decimal Convert(decimal amount)
        {
            return ConvertBtwCurrencies(amount, Base_Code, Default_Code);
        }

        public static string ConvertD(decimal amount)
        {
            return Display(Convert(amount), GetDefaultCurrency().Icon);
        }

        public static void Init()
        {
            Currencies.PushIf(new Currency("NGN", "\u20a6", true, 1M, "Nigerian Naira"), "Code");
            Currencies.PushIf(new Currency("USD", "\u0024", false, 0.0026109661M, "US Dollar"), "Code");
            Currencies.PushIf(new Currency("AED", "\u062f\u002e\u0625", false, 0.0111M, "Arab Emirate Dirham"), "Code");
            Currencies.PushIf(new Currency("GHS", "\u0047\u0048\u20b5", false, 0.0172M, "Ghanaian Cedi"), "Code");
            Currencies.PushIf(new Currency("GBP", "\u00a3", false, 0.002222M, "Great British Pound"), "Code");
            Currencies.PushIf(new Currency("EUR", "€", false, 0.0028M, "Euro"), "Code");
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
                decimal rate = (decimal)obj["amount"];
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
