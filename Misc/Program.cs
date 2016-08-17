using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using Extensions.Models;
using Extensions.Heuristics.Meta;

namespace Misc
{
    public class Program
    {

        public static void Main(String[] args)
        {
            //byte[] arr = new byte[] {
            //    2,7,4,5,0,3,6,1 //0, 5, 7, 2, 6, 3, 1, 4
            //}; ;
            //Console.WriteLine(arr.Join(","));
            //Console.WriteLine(fitnessFunction(arr));


            //arr = mutationFunc(arr);
            //Console.WriteLine(arr.Join(","));
            //Console.WriteLine(fitnessFunction(arr));
            //Console.Read();
            //    double fitness = fitnessFunction(arr);
            //    Console.WriteLine(fitness);
            //Console.Read();
            Hive<byte[]>.Create(mutationFunc, fitnessFunction,
                clone, Selection.RoulleteWheel);
            Hive<byte[]>.Bee<byte[]> best = Hive<byte[]>.FullIteration(generateSolution, 150, true);
            Console.WriteLine(best.Food.Join(","));
            Console.Read();
        }

        public static byte[] generateSolution()
        {
            List<byte> ret = new List<byte>();
            for (byte i = 0; i < 8; i++)
            {
                byte b = byte.MinValue;
            line1:
                b = Convert.ToByte(Math.Floor(Number.Rnd() * 8));
                if (ret.Contains(b)) goto line1;
                ret.Add(b);
            }
            return ret.ToArray();
        }

        public static byte[] clone(byte[] queens)
        {
            List<byte> ret = new List<byte>();
            for (int i = 0; i < queens.Length; i++)
            {
                ret.Add(Convert.ToByte(queens[i] + 0));
            }
            return ret.ToArray();
        }

        public static byte[] mutationFunc(byte[] queens)
        {
        line1:
            byte i = Convert.ToByte(Math.Floor(Number.Rnd() * 8));
            byte j = Convert.ToByte(Math.Floor(Number.Rnd() * 8));
            if (i == j) goto line1;
            byte extra = (byte)(queens[i] + 0);
            queens[i] = (byte)(queens[j] + 0);
            queens[j] = (byte)(extra + 0);
            return queens;
        }

        public static double fitnessFunction(byte[] queens)
        {
            byte ret = 0;
            for (byte i = 0; i < queens.Length; i++)
            {
                for (byte j = 0; j < queens.Length; j++)
                {
                    if (j > 0 && Math.Abs((byte)(queens[j] - queens[j - 1])) == 1) ret++;
                    byte a = (byte)Math.Abs(j - i);
                    byte b = (byte)Math.Abs(queens[j] - queens[i]);
                    if ((a == b || j == i || queens[i] == queens[j]) && (i != j))
                    {
                        ret++;
                    }
                }
            }
            return Convert.ToDouble(Convert.ToDouble(ret)); // / (double)64
        }
    }
}
