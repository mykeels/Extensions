using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using Extensions.Models;

namespace Misc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> texts = new List<string>() { "a", "b", "c" };
            Console.WriteLine(texts.AsEnumerable().Backwards().ToList().ToJson());
            Console.Read();
        }
    }
}
