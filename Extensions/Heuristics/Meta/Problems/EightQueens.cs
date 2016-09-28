using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.Heuristics.Meta.Abc;
using Extensions.Heuristics.Meta.GA;

namespace Extensions.Heuristics.Meta.Problems
{
    public class EightQueens
    {
        public static byte[] GenerateNewCandidateSolution()
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

        public static byte[] Clone(byte[] queens)
        {
            
            List<byte> ret = new List<byte>();
            for (int i = 0; i < queens.Length; i++)
            {
                ret.Add(Convert.ToByte(queens[i] + 0));
            }
            return ret.ToArray();
        }

        public static byte[] FindNeighbor(byte[] queens)
        {
            //Console.WriteLine("Find Neighbor");
            //Console.WriteLine(queens.Join(", "));
        line1:
            byte i = Convert.ToByte(Math.Floor(Number.Rnd() * 8));
            byte j = Convert.ToByte(Math.Floor(Number.Rnd() * 8));
            if (i == j) goto line1;
            byte extra = (byte)(queens[i] + 0);
            queens[i] = (byte)(queens[j] + 0);
            queens[j] = (byte)(extra + 0);
            //Console.WriteLine(queens.Join(", "));
            return queens;
        }

        public static double GetSolutionFitness(byte[] queens)
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
            return Convert.ToDouble(ret);
        }

        public static Configuration<byte[]> GetConfiguration()
        {
            Configuration<byte[]> config = new Configuration<byte[]>();
            config.CloneFunction = EightQueens.Clone;
            config.InitializeSolutionFunction = EightQueens.GenerateNewCandidateSolution;
            config.MutationFunction = EightQueens.FindNeighbor;
            config.NoOfIterations = 1500;
            config.ObjectiveFunction = EightQueens.GetSolutionFitness;
            config.SelectionFunction = Selection.RoulleteWheel;
            return config;
        }

        public static byte[] GetCorrectSolutionABC()
        {
            Hive<byte[], Bee<byte[]>> hive = new Hive<byte[], Bee<byte[]>>();
            hive.Create(GetConfiguration());
            byte[] food = (byte[])hive.FullIteration();
            return food;
        }

        public static byte[] GetCorrectSolutionGA()
        {
            GeneticAlgorithm<byte[]> ga = new GeneticAlgorithm<byte[]>((byte[] a, byte[] b) =>
            {
                return GA.CrossOver.Uniform(a.AsEnumerable(), b.AsEnumerable()).ToArray();
            });
            ga.Create(GetConfiguration());
            byte[] food = ga.FullIteration();
            return food;
        }

        public static byte[] GetCorrectSolutionHC()
        {
            HillClimb<byte[]> hc = new HillClimb<byte[]>();
            hc.Create(GetConfiguration());
            byte[] food = hc.FullIteration();
            return food;
        }

        public static byte[] GetCorrectSolutionSA()
        {
            SimulatedAnnealing<byte[]> sa = new SimulatedAnnealing<byte[]>();
            sa.Create(GetConfiguration());
            byte[] food = sa.FullIteration();
            return food;
        }
    }
}
