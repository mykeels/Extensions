using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Extensions.Models
{
    public partial class Currency
    {
        public class Manager
        {
            public List<Currency> Currencies { get; set; }
            private string Base_Code { get; set; }
            private string Default_Code { get; set; }

            public Manager(string baseCode)
            {
                this.Base_Code = baseCode;
                this.Refresh();
            }

            private void Init()
            {
                Currencies = new List<Currency>();
                Currencies.PushIf(new Currency("NGN", "\u20a6", false, 1M, "Nigerian Naira"), "Code");
                Currencies.PushIf(new Currency("USD", "\u0024", false, 0.0026109661M, "US Dollar"), "Code");
                Currencies.PushIf(new Currency("AED", "\u062f\u002e\u0625", false, 0.0111M, "Arab Emirate Dirham"), "Code");
                Currencies.PushIf(new Currency("GHS", "\u0047\u0048\u20b5", false, 0.0172M, "Ghanaian Cedi"), "Code");
                Currencies.PushIf(new Currency("GBP", "\u00a3", false, 0.002222M, "Great British Pound"), "Code");
                Currencies.PushIf(new Currency("EUR", "€", false, 0.0028M, "Euro"), "Code");
                var currency = Currencies.Where((c) => c.Code == Base_Code).FirstOrDefault();
                if (currency != null) currency.Base = true;
            }

            public void Refresh()
            {
                this.Init();
                string path = "http://beta.wakanow.com/activities/CurrencyService.aspx?method=GetExchangeRates";
                var str = Currencies.Select("Code").Join(",");
                string results = Api.Get(path + "&from=" + Base_Code + "&to=" + str);
                JArray arr = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(results);
                foreach (var obj in arr)
                {
                    string code = (string)obj["code"];
                    decimal rate = (decimal)obj["amount"];
                    var currencies = (from pp in Currencies where pp.Code.Equals(code) select pp);
                    if (currencies.Count() > 0) currencies.First().Rate = rate;
                }
            }
            
            public string GetIcon(string code)
            {
                return this.Currencies.Where((c) => c.Code.Equals(code)).FirstOrDefault(new Currency()).Icon;
            }

            public Currency GetBaseCurrency()
            {
                return this.Currencies.Where((c) => c.Base).First();
            }

            public Currency SetBaseCurrency(string code)
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

            public Currency GetDefaultCurrency()
            {
                return this.Currencies.Where((c) => c.Code == Default_Code).FirstOrDefault();
            }

            public Currency GetCurrencyByIcon(string icon)
            {
                return this.Currencies.Where((c) => c.Icon == icon).FirstOrDefault();
            }

            public Currency GetCurrency(string code)
            {
                return this.Currencies.Where((c) => c.Code == code).FirstOrDefault();
            }

            public void SetRate(string code, decimal rate)
            {
                this.GetCurrency(code).Rate = rate;
            }

            public decimal GetRate(string code)
            {
                return this.GetCurrency(code).Rate;
            }

            public decimal ConvertFromBase(decimal base_amt, string code)
            {
                if (code == Base_Code) return base_amt;
                return Math.Round(base_amt * GetRate(code), 2);
            }

            public decimal ConvertToBase(decimal amount, string code)
            {
                if (code == Base_Code) return amount;
                return Math.Round(amount / GetRate(code), 2);
            }

            public decimal ConvertBtwCurrencies(decimal amount1, string code1, string code2)
            {
                decimal amt_in_base = ConvertToBase(amount1, code1);
                return this.ConvertFromBase(amt_in_base, code2);
            }

            public string Display(decimal amount, string code)
            {
                return this.GetIcon(code) + " " + Math.Round(amount, 2);
            }

            public decimal Convert(decimal amount)
            {
                return this.ConvertBtwCurrencies(amount, Base_Code, Default_Code);
            }

            public string ConvertD(decimal amount)
            {
                return this.Display(Convert(amount), GetDefaultCurrency().Icon);
            }

        }
    }
}
