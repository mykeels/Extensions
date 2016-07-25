using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.ServiceModel;
using Extensions;
using Extensions.Models;
using Test_Extensions.MouseServiceReference;

namespace Test_Extensions.MouseCtrl
{
    public class Stuff
    {
        static Form form1 = null;
        public static MouseServiceClient otherClient { get; set; }
        public static string otherPCName { get; set; }

        public static void Startup()
        {
            Console.WriteLine("Name:\t" + Environment.MachineName + "\n");
            GetIpAddresses().ForEach((ip) =>
            {
                if (!String.IsNullOrEmpty(ip)) Console.WriteLine("IP:\t" + ip);
            });
            Console.WriteLine("Service Started ...");
            //Console.WriteLine("1:\tListen");
            //string option = Console.ReadLine();

            Console.Write("Enter Remote IP Address of Controller PC: ");
            string otherIPAddress = Console.ReadLine();

            otherClient = new MouseServiceClient("BasicHttpBinding_IMouseService",
                new EndpointAddress(new Uri(String.Format(GetServiceUrl().Replace("localhost", "{0}").Replace("[::]", "{0}"), otherIPAddress))));
            System.Threading.Thread.Sleep(1000);
            if (MouseService.state == MouseService.State.Pending)
            {
                MouseService.state = MouseService.State.Listening;
                StartListening();
            }
        }
        public static void testMouseCtrl()
        {
            Promise<bool>.Create(() =>
            {
                if (form1 == null || form1.IsDisposed || !form1.Enabled)
                {
                    form1 = new Form();
                    form1.Width = Screen.AllScreens.ToList().Sum((screen) =>
                    {
                        return screen.WorkingArea.Width;
                    });
                    form1.Height = Screen.AllScreens.ToList().Max((screen) =>
                    {
                        return screen.WorkingArea.Height + 100;
                    });
                    form1.BackColor = Color.White;
                    form1.Opacity = 0.3;
                    form1.FormBorderStyle = FormBorderStyle.None;
                    form1.Location = new Point(0, -100);
                    form1.KeyUp += (object sender, KeyEventArgs e) =>
                    {
                    //Console.Write(Char.ConvertFromUtf32(e.KeyValue));
                    otherClient.KeyUp(e.KeyCode, e.Control, e.KeyCode == Keys.LWin, e.Alt, e.Shift);
                        if (e.Control && e.KeyCode == Keys.M) form1.WindowState = FormWindowState.Minimized;
                        if (e.Alt && e.Control && e.KeyCode == Keys.F4)
                        {
                            otherClient.StopListening();
                            Console.WriteLine("Exiting Program ...");
                            Environment.Exit(0);
                        }
                        e.SuppressKeyPress = true;
                    };
                    form1.MouseDown += (object sender, MouseEventArgs e) =>
                    {
                        if (e.Button == MouseButtons.Left) otherClient.MouseLeftDown();
                        else if (e.Button == MouseButtons.Middle) otherClient.MouseMiddleDown();
                        else if (e.Button == MouseButtons.Right) otherClient.MouseRightDown();
                    };
                    form1.MouseUp += (object sender, MouseEventArgs e) =>
                    {
                        if (e.Button == MouseButtons.Left) otherClient.MouseLeftUp();
                        else if (e.Button == MouseButtons.Middle) otherClient.MouseMiddleUp();
                        else if (e.Button == MouseButtons.Right) otherClient.MouseRightUp();
                    };
                    form1.MouseWheel += (object sender, MouseEventArgs e) =>
                    {
                        otherClient.MouseWheel(e.Delta);
                    };
                    form1.Resize += (object sender, EventArgs e) =>
                    {
                        if (form1.WindowState == FormWindowState.Minimized)
                        {
                            Console.WriteLine("Sent STOP LISTENING ...");
                            otherClient.StopListening();
                        }
                        else if (form1.WindowState == FormWindowState.Normal)
                        {
                            Console.WriteLine("Sent START LISTENING ...");
                            otherClient.StartListening();
                        }
                    };
                    if (form1 != null) Application.Run(form1);
                }
                return false;
            });
        }

        public static void StartListening()
        {
            Console.WriteLine("Now Listening ... ");
            otherClient.PushWhoIAm(GetPCName());
            while (MouseService.state == MouseService.State.Listening)
            {
                try
                {
                    Mouse.Win32.Point pt = new Mouse.Win32.Point();
                    pt = otherClient.GetMousePosition();
                    int addX = 0;
                    if (MouseService.currentScreen == MouseService.Screen.Secondary) addX = Screen.PrimaryScreen.WorkingArea.Width;
                    Mouse.SetCursorPosition(pt.x + addX, pt.y);
                    //Console.WriteLine("Mouse Coordinates: " + pt.x + "\t" + pt.y);
                    System.Threading.Thread.Sleep(50);
                }
                catch (Exception ex) { }
            }
            Console.Write("I have Stopped Listening ...");
        }

        public static string GetServiceUrl()
        {
            return MouseService.serviceUrl + "?wsdl";
        }

        public static List<string> GetIpAddresses()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.Select((ip) =>
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                    return localIP;
                }
                return null;
            }).ToList();
        }

        public static string GetIpAddress()
        {
            return GetIpAddresses().FirstOrDefault("");
        }

        public static string GetPCName()
        {
            return Environment.MachineName;
        }
    }
}
