using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CdnBundle;
using NUnit;
using NUnit.Framework;
using Extensions;

namespace Tests
{
    [TestFixture]
    public class Program
    {
        public Program()
        {
            if (!System.IO.Directory.Exists("JS")) System.IO.Directory.CreateDirectory("JS");
            if (!System.IO.Directory.Exists("CSS")) System.IO.Directory.CreateDirectory("CSS");
        }

        public static void Main(string[] args)
        {
            Program p = new Program();

            var cssBundle = p.getCssBundles();
            var jsBundle = p.getJsBundles();

            //p.bundleLoadTypeShouldBeCdn();
            p.bundleLoadTypeShouldBeLocal();
            p.bundlesShouldLocallyIfLoadedBefore();
            p.bundlesShouldNotBeOverWrittenIfStillValid();
            Console.Read();
        }

        [Test]
        public bool bundleLoadTypeShouldBeLocal()
        {
            var item = new Bundle("~/css/lightbox.min.css", Bundle.BundleType.CSS, false);
            string response = item.Load();
            Assert.That(item.getLoadType() == "LOCAL", "This Bundle should load from the FileSystem");
            return true;
        }

        [Test]
        public bool bundleLoadTypeShouldBeCdn()
        {
            var item = new Bundle("https://cdnjs.cloudflare.com/ajax/libs/lightbox2/2.8.2/css/lightbox.min.css", "~/css/lightbox.min.css", Bundle.BundleType.CSS, false);
            if (System.IO.File.Exists(item.getLocalFilePath()))
            {
                System.IO.File.Delete(item.getLocalFilePath());
            }
            string response = item.Load();
            Assert.That(item.getLoadType() == "CDN", "This Bundle should load from the CDN");
            return true;
        }

        [Test]
        public bool bundlesShouldLocallyIfLoadedBefore()
        {
            var jsBundle = getJsBundles();
            int count = jsBundle.Count;
            jsBundle = jsBundle.Where(js => !String.IsNullOrEmpty(js.cdnUrl)).ToList();
            jsBundle = jsBundle.Where(js => !String.IsNullOrEmpty(js.localUrl) && !System.IO.File.Exists(js.getLocalFilePath())).ToList();
            Console.WriteLine("Loaded and Bundled " + jsBundle.Count + " of " + count + " JS Files");
            string response = jsBundle.Load();
            Console.WriteLine(jsBundle.Select((js) => js.getLoadType()).ToList().Join(", "));
            
            string secondResponse = jsBundle.Load();
            Assert.That(jsBundle.Select(js => js.getLoadType())
                .All(loadtype => loadtype == "LOCAL"), "All Bundles should load from the Local FileSystem after the first load");
            Console.WriteLine(jsBundle.Select((js) => js.getLoadType()).ToList().Join(", "));
            return true;
        }

        [Test]
        public bool bundlesShouldNotBeOverWrittenIfStillValid()
        {
            var jsBundle = getJsBundles();
            int count = jsBundle.Count;
            foreach (var item in jsBundle)
            {
                if (System.IO.File.Exists(item.getLocalFilePath()))
                {
                    System.IO.File.Delete(item.getLocalFilePath());
                }
            }
            jsBundle = jsBundle.Where(js => !String.IsNullOrEmpty(js.cdnUrl)).ToList();
            jsBundle = jsBundle.Where(js => !String.IsNullOrEmpty(js.localUrl) && !System.IO.File.Exists(js.getLocalFilePath())).ToList();
            Console.WriteLine("Loaded and Bundled " + jsBundle.Count + " of " + count + " JS Files");
            string response = jsBundle.Load();
            Console.WriteLine();
            List<DateTime> jsFileDates = jsBundle.Select((js) => js.getLocalFileInfo().CreationTime).ToList();
            Console.WriteLine("Now Sleeping for 2000 Milliseconds");
            System.Threading.Thread.Sleep(2000);
            string secondResponse = jsBundle.Load();
            List<DateTime> jsFileDatesTwo = jsBundle.Select((js) => js.getLocalFileInfo().CreationTime).ToList();
            for (int i = 0; i < jsFileDates.Count; i++)
            {
                Assert.That(jsFileDates[i] == jsFileDatesTwo[i], "Local Files OverWritten even when valid");
            }
            Console.WriteLine("Local File Dates are Solid and Good");
            return true;
        }

        public List<Bundle> getCssBundles()
        {
            List<Bundle> cssBundle = new List<Bundle>();
            cssBundle.Add(new Bundle("https://cdnjs.cloudflare.com/ajax/libs/lightbox2/2.8.2/css/lightbox.min.css", "~/css/lightbox.min.css", Bundle.BundleType.CSS, false));
            cssBundle.Add(new Bundle("https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.5/css/bootstrap.min.css", "~/css/bootstrap.min.css", Bundle.BundleType.CSS, false));
            cssBundle.Add(new Bundle("http://code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.min.css", "~/css/jquery-ui.min.css", Bundle.BundleType.CSS, false));
            cssBundle.Add(new Bundle("http://cdn.bootcss.com/toastr.js/2.1.2/toastr.min.css", "~/css/toastr.min.css", Bundle.BundleType.CSS, false));
            cssBundle.Add(new Bundle("https://cdnjs.cloudflare.com/ajax/libs/jquery.smartmenus/1.0.0/addons/bootstrap/jquery.smartmenus.bootstrap.min.css", "~/css/jquery.smartmenus.bootstrap.min.css", Bundle.BundleType.CSS, false));
            cssBundle.Add(new Bundle("https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick.min.css", "~/css/slick.min.css", Bundle.BundleType.CSS, false));
            cssBundle.Add(new Bundle("", "~/css/style.css", Bundle.BundleType.CSS, false));
            cssBundle.Add(new Bundle("", "~/css/calendar.css", Bundle.BundleType.CSS, false));
            cssBundle.Add(new Bundle("", "~/css/mobile.css", Bundle.BundleType.CSS, false));
            return cssBundle;
        }

        public List<Bundle> getJsBundles()
        {
            List<Bundle> jsBundle = new List<Bundle>();
            jsBundle.Add(new Bundle("https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js", "~/JS/jquery-2.1.4.min.js", Bundle.BundleType.JavaScript, false));
            jsBundle.Add(new Bundle("https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js", "~/JS/jquery-ui.min.js", Bundle.BundleType.JavaScript, false));
            jsBundle.Add(new Bundle("https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js", "~/JS/bootstrap.min.js", Bundle.BundleType.JavaScript, false));
            jsBundle.Add(new Bundle("https://cdnjs.cloudflare.com/ajax/libs/jquery.smartmenus/1.0.0/jquery.smartmenus.min.js", "~/JS/jquery.smartmenus.min.js", Bundle.BundleType.JavaScript, false));
            jsBundle.Add(new Bundle("http://labs.rampinteractive.co.uk/touchSwipe/jquery.touchSwipe.min.js", "~/JS/jquery.touchSwipe.min.js", Bundle.BundleType.JavaScript, false));
            jsBundle.Add(new Bundle("", "~/js/jquery.slideandswipe.min.js", Bundle.BundleType.JavaScript, false));
            jsBundle.Add(new Bundle("https://cdnjs.cloudflare.com/ajax/libs/jquery.smartmenus/1.0.0/addons/bootstrap/jquery.smartmenus.bootstrap.min.js", "~/JS/jquery.smartmenus.bootstrap.min.js", Bundle.BundleType.JavaScript, false));
            jsBundle.Add(new Bundle("https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.4.5/angular.min.js", "~/JS/angular.min.js", Bundle.BundleType.JavaScript, false));
            jsBundle.Add(new Bundle("http://static.wakanow.com/common/general/js/all.js", "~/JS/all.js", Bundle.BundleType.JavaScript, false));
            return jsBundle;
        }
    }
}
