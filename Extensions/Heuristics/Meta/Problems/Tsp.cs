using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta.Problems
{
    public class Tsp
    {
        public class City
        {
            public double X;
            public double Y;
            public string Name;
        }

        private List<City> Cities = new List<City>();

        public Tsp(List<City> cities)
        {
            this.Cities = cities;
        }

        public double GetDistance(int x, int y)
        {
            City a = this.Cities[x];
            City b = this.Cities[y];
            return Math.Sqrt(((b.X - a.X) * (b.X - a.X)) + ((b.Y - a.Y) * (b.Y - a.Y)));
        }

        public double GetDistance(City a, City b)
        {
            return Math.Sqrt(((b.X - a.X) * (b.X - a.X)) + ((b.Y - a.Y) * (b.Y - a.Y)));
        }

        public double GetRouteDistance(int[] sequence)
        {
            double total = 0;
            for (int i = 1; i < this.Cities.Count; i++)
            {
                total += GetDistance(sequence[i], sequence[i - 1]);
            }
            return total;
        }

        public double GetRouteDistance(List<City> cities)
        {
            double total = 0;
            for (int i = 1; i < this.Cities.Count; i++)
            {
                total += GetDistance(this.Cities[i], this.Cities[i - 1]);
            }
            return total;
        }

        public int[] Clone(int[] sequence)
        {
            if (sequence == null) throw new Exception("int[] sequence should not be null");
            int[] ret = new int[sequence.Length];
            for (int i = 0; i < sequence.Length; i++)
            {
                ret[i] = sequence[i] + 0;
            }
            return ret;
        }

        public int[] GetNewSolution()
        {
            List<int> ret = new List<int>();
            for (int i = 0; i < Cities.Count; i++)
            {
                ret.Add(i);
            }
            ret = ListExtensions.Shuffle(ret);
            return ret.ToArray();
        }

        public int[] MutateSwap(int[] sequence)
        {
            line1:
            if (sequence.Length <= 0) throw new Exception("int[] sequence length should be more than 1");
            int a = Convert.ToInt32(Math.Floor(Number.Rnd() * sequence.Length));
            int b = Convert.ToInt32(Math.Floor(Number.Rnd() * sequence.Length));
            if (a == b) goto line1;
            int proxy = sequence[a] + 0;
            sequence[a] = sequence[b] + 0;
            sequence[b] = proxy + 0;
            return sequence;
        }

        public int[] MutateLShift(int[] sequence)
        {
            int[] ret = new int[sequence.Length];
            int shifts = Convert.ToInt32(Math.Floor(Number.Rnd() * Math.Min(5, sequence.Length - 1)));
            for (int i = 0; i < sequence.Length - shifts; i++)
            {
                ret[i] = sequence[i + shifts];
            }
            for (int i = sequence.Length - shifts; i < sequence.Length; i++)
            {
                ret[i] = sequence[i - (sequence.Length - shifts)];
            }
            return ret;
        }

        public int[] MutateInsert(int[] sequence)
        {
            List<int> xsequence = sequence.ToList();
            int x = xsequence.Random();
            xsequence.Remove(x);
            xsequence.Insert(Convert.ToInt32(Math.Floor(Number.Rnd() * xsequence.Count)), x);
            return xsequence.ToArray();
        }

        public int[] MutateAny(int[] sequence)
        {
            double r = Number.Rnd();
            if ( r< 0.33)
            {
                return MutateSwap(sequence);
            }
            else if (r < 0.66)
            {
                return MutateLShift(sequence);
            }
            else
            {
                return MutateInsert(sequence);
            }
        }

        public Configuration<int[]> GetDefaultConfig()
        {
            Configuration<int[]> config = new Configuration<int[]>();
            config.CloneFunction = this.Clone;
            config.EnforceHardObjective = false;
            config.InitializeSolutionFunction = this.GetNewSolution;
            config.Movement = Search.Direction.Optimization;
            config.MutationFunction = this.MutateAny;
            config.NoOfIterations = 500000;
            config.ObjectiveFunction = this.GetRouteDistance;
            config.PopulationSize = 30;
            config.SelectionFunction = Selection.RoulleteWheel;
            config.WriteToConsole = true;
            config.ConsoleWriteInterval = 10;
            return config;
        }

        public int[] Solve()
        {
            Abc.Hive<int[], Abc.Bee<int[]>> hive = new Abc.Hive<int[], Abc.Bee<int[]>>(10, 0.1);
            hive.Create(GetDefaultConfig());
            return hive.FullIteration();
        }

        public static List<City> GetRandomProblem(int noOfCities = 20)
        {
            List<Tsp.City> cities = new List<Tsp.City>();
            for (int i = 0; i < noOfCities; i++)
            {
                cities.Add(new Tsp.City()
                {
                    Name = "City " + (i + 1),
                    X = Number.Rnd() * 500,
                    Y = Number.Rnd() * 800
                });
            }
            return cities;
        }
    }
}
