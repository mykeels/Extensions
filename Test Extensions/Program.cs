using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using Extensions.Models;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ServiceModel;

namespace Test_Extensions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine("iWueiQ+nLTJ8h6/BBgPUzg==".Replace(" ", "+").Decrypt("DestsAfrica"));
            //Currency.Refresh();
            foreach (var code in Currency.Currencies.Select((c) => c.Code).ToList())
            {
                Currency.Manager manager = new Currency.Manager(code);
                Console.WriteLine("Base Currency: " + code);
                Console.WriteLine(manager.Currencies.ToJson(true));
            }

            ServiceHost host = new ServiceHost(typeof(MouseService));
            MouseService.serviceUrl = host.BaseAddresses[0].ToString();
            host.Open();
            Application.ApplicationExit += (object sender, EventArgs e) =>
            {
                MouseService.state = MouseService.State.Pending;
                MouseCtrl.Stuff.otherClient.StopListening();
                host.Close();
            };
            MouseCtrl.Stuff.Startup();
            
            //System.Xml.Linq.XDocument pressData = JsonConvert.DeserializeXNode(System.IO.File.ReadAllText(@"C:\Wakanow\GenericPages\GenericPages\Generics\Scripts\JSON\pressdata.json"));
            //Console.WriteLine(pressData.ToXElement().ToDataTable().ToCSV("\t"));

            var reader = new Extensions.TextReader();
            Console.SetIn(reader);
            Console.WriteLine(reader.ReadPassword());
            /*Mail.Send("michaelik@wakanow.com", "Test via Cloud 2 Wait for it", "Hello World for Africa!", Site.AppSettings("EmailFromAddress"), "Destinations Africa",
                new List<string>
            {
                    @"michaelikechim@gmail.com"
            });*/
            /*Mail.SendViaCloud("michaelik@wakanow.com", "Test via Cloud 2 Wait for it", "Hello World for Africa!", Site.AppSettings("EmailFromAddress"), null, 
                new List<string>
            {
                    @"michaelikechim@gmail.com"
            });*/
            IPUser.CreateNewUser();
            Location.Country.GetCountries();
            Location.City.GetCities();
            Location.Continent_Country.GetContinentCountryPairs();

            RichTextBox rt = new RichTextBox();
            rt.Text = p.generateEncodeURI();
            Form form = p.previewInForm(rt);
            form.ShowDialog();
            Console.Read();
            /*Api.Execute("http://www.wakanow.com/en-ng/things-to-do").Success((browser) =>
            {

                Form form = p.previewInForm((WebBrowser)browser, ((WebBrowser)browser).DocumentTitle);
                ((WebBrowser)browser).DocumentCompleted += delegate
                {
                    ((WebBrowser)browser).Height = ((WebBrowser)browser).Document.Body.ScrollRectangle.Height;
                    ((WebBrowser)browser).Width = ((WebBrowser)browser).Parent.Width;
                    ((WebBrowser)browser).Parent.Resize += delegate
                    {
                        ((WebBrowser)browser).Width = ((WebBrowser)browser).Parent.Width;
                    };
                    form.Text = ((WebBrowser)browser).DocumentTitle;
                };
                form.ShowDialog();
            }).Error((ex) =>
            {

            });*/

            //Currency.Refresh();
            List<Currency> currencies = new List<Currency>();
            Console.WriteLine(currencies.FirstOrDefault(new Currency()).ToJson());
            Console.Read();
            Console.WriteLine("my name is ikechi michael".CapitaliseEachWord());
            Test.testPush();
            Test.testRandomSingle();
            Test.testRandom();
            Test.testSort();
            Test.testSelect();
            Test.testJoin();
            Test.testSum();
            Test.testFrom();
            Test.testWhere();
            Test.testFlatten();
            Test.testFirst();
            Test.testLast();
            Test.testPushRange();
            Test.testContains();
            Test.testDistinct();
            Test.testPaginate();
            Console.Read();
        }


        string generateEncodeURI()
        {
            string ret = "";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("\"", "-");
            dict.Add(" ", "-");
            dict.Add("&", "and");
            for (int i = 1; i < 48; i++)
            {
                string s = Char.ConvertFromUtf32(i).ToString();
                dict.AddOnce<string, string>(s, "-");
            }
            for (int i = 58; i < 65; i++)
            {
                string s = Char.ConvertFromUtf32(i).ToString();
                dict.AddOnce<string, string>(s, "-");
            }
            for (int i = 91; i < 97; i++)
            {
                string s = Char.ConvertFromUtf32(i).ToString();
                dict.AddOnce<string, string>(s, "-");
            }
            for (int i = 123; i < 200; i++)
            {
                string s = Char.ConvertFromUtf32(i).ToString();
                dict.AddOnce<string, string>(s, "-");
            }
            dict.ToList().ForEach((pair) =>
            {
                ret += "@urlresult = REPLACE(@urlresult, '" + pair.Key + "', '" + pair.Value + "'), \n";
            });
            return ret;
        }

        Form previewInForm(Control ctrl, string title = "Preview", int width = 600, int height = 400)
        {
            Form form = new Form();
            form.Controls.Add(ctrl);
            form.Width = width;
            form.Height = height;
            form.Text = title + " | " + ctrl.GetType().Name;
            ctrl.Dock = DockStyle.Fill;
            //ctrl.Parent = form;
            //form.AutoScroll = true;
            return form;
        }
    }
}
