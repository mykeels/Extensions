using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.Heuristics.Meta.Abc;
using Extensions.Heuristics.Meta.GA;

namespace Extensions.Heuristics.Meta.Problems
{
    public class Knapsack
    {
        public List<Item> Items = new List<Item>();
        public List<double> KnapsackWeights = new List<double>();
        public int NoOfKnapsacks, NoOfItems;
        public int Goal = 0;

        public class Item
        {
            public double value { get; set; }
            public List<double> weights { get; set; }

            public Item()
            {

            }

            public Item(double value, List<double> weights)
            {
                this.value = value;
                this.weights = weights;
            }

            public bool Equals(Item item)
            {
                if (item.value == this.value && item.weights.Count == this.weights.Count)
                {
                    for (int i = 0; i < item.weights.Count; i++)
                    {
                        if (item.weights[i] != this.weights[i]) return false;
                    }
                    return true;
                }
                return false;
            }
        }

        public List<int> GetInitialSolution()
        {
            line1:
            List<int> ret = new List<int>();
            while (ret.Count < 1)
            {
                int x = (int)Math.Floor(Number.Rnd() * Items.Count);
                if (!ret.Contains(x)) ret.Add(x);
            }
            if (this.GetFitness(ret) == Double.MaxValue)
            {
                ret.Clear();
                goto line1;
            }
            return ret.Distinct().ToList();
        }

        public double GetTotalWeight(List<int> solution, int kIndex)
        {
            double sumWeight = 0;
            foreach (int i in solution)
            {
                sumWeight += Items[i].weights[kIndex];
            }
            return sumWeight;
        }

        public double GetFitness(IEnumerable<int> solution)
        {
            double sumValue = 0;
            List<double> sumWeights = new List<double>();
            for (int i = 0; i < KnapsackWeights.Count; i++)
            {
                sumWeights.Add(0);
            }
            foreach (int i in solution)
            {
                int xi = 0;
                foreach (var weight in Items[i].weights)
                {
                    sumWeights[xi] += weight;
                    xi++;
                }
                sumValue += Items[i].value;
            }
            bool allKnapsacksOkay = true;
            int wi = 0;
            foreach (var weight in sumWeights)
            {
                if (weight > KnapsackWeights[wi]) allKnapsacksOkay = false;
                wi++;
            }
            if (allKnapsacksOkay) return sumValue;
            else return Double.MaxValue;
        }

        public List<int> GetKnapsackIndicesThatSupportWeight(double weight)
        {
            List<int> ret = new List<int>();
            for (int index = 0; index < Items.Count; index++)
            {
                if (Items[index].weights.First() < weight) ret.Add(index);
            }
            return ret;
        }

        public List<int> Mutate(List<int> solution)
        {
            double r = Number.Rnd();
            List<int> sol = null;
            if (r < 0.3) sol = ReplaceTwoWithOne(solution).ToList();
            else if (r < 0.6) sol = ReplaceOneWithTwo(solution).ToList();
            else sol = ReplaceOneWithOne(solution).ToList();
            return sol;
        }

        private IEnumerable<int> ReplaceOneWithOne(IEnumerable<int> solution)
        {
            List<int> ret = Clone(solution.ToList()).ToList();
            if (ret != null)
            {
                if (ret.Count >= 1)
                {
                    int i = (int)Math.Floor(Number.Rnd() * solution.Count());
                    int val = (int)Math.Floor(Number.Rnd() * Items.Count);
                    if (!solution.Contains(val)) ret[i] = val;
                }
            }
            return ret.Distinct().AsEnumerable();
        }

        private IEnumerable<int> ReplaceOneWithTwo(IEnumerable<int> solution)
        {
            if (solution != null)
            {
                if (solution.Count() >= 1)
                {
                    List<int> ret = Clone(solution.ToList()).ToList();
                    int i = (int)Math.Floor(Number.Rnd() * ret.Count());
                    line1:
                    int val1 = (int)Math.Floor(Number.Rnd() * Items.Count());
                    int val2 = (int)Math.Floor(Number.Rnd() * Items.Count());
                    if (val1 == val2 || ret.Contains(val1) || ret.Contains(val2)) goto line1;
                    ret[i] = val1;
                    ret.Insert(i, val2);
                    return ret.Distinct().AsEnumerable();
                }
                throw new Exception("No of items in Solution should be at least 1");
            }
            throw new Exception("Solution should not be null");
        }

        private IEnumerable<int> ReplaceTwoWithOne(IEnumerable<int> solution)
        {
            List<int> ret = Clone(solution.ToList()).ToList();
            if (ret != null)
            {
                if (ret.Count > 1)
                {
                    line1:
                    int i1 = (int)Math.Floor(Number.Rnd() * ret.Count);
                    int i2 = (int)Math.Floor(Number.Rnd() * ret.Count);
                    if (i1 == i2)
                    {
                        goto line1;
                    }
                    ret.RemoveAt(Math.Max(i1, i2));
                    ret.RemoveAt(Math.Min(i1, i2));
                    int val = (int)Math.Floor(Number.Rnd() * Items.Count);
                    ret.Insert(Math.Min(i1, i2), val);
                }
            }
            return ret.Distinct().AsEnumerable();
        }

        public List<int> Clone(List<int> solution)
        {
            List<int> ret = new List<int>();
            foreach (int x in solution)
            {
                ret.Add(x + 0);
            }
            return ret;
        }

        public static Knapsack ReadProblemTypeOne(string filepath)
        {
            int i = 0;
            int countItemValues = 0, countItemsWeights = 0;
            Knapsack ret = new Knapsack();
            List<Item> retItems = new List<Item>();
            int knapsackCapacitiesLineReached = 0;
            
            string[] lines = System.IO.File.ReadAllLines(filepath);
            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index];
                line = line.TrimStart(' ');
                string[] ss = line.Split(' ');
                if (i == 0)
                {
                    ret.NoOfKnapsacks = Convert.ToInt32(ss[0]);
                    ret.NoOfItems = Convert.ToInt32(ss[1]);
                }
                else
                {
                    if (countItemValues < ret.NoOfItems)
                    {
                        foreach (string sss in ss)
                        {
                            if (sss == "//") break;
                            else
                            {
                                if (sss != "")
                                {
                                    Item item = new Item(Convert.ToDouble(sss), new List<double>());
                                    ret.Items.Add(item);
                                    countItemValues++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (knapsackCapacitiesLineReached < ret.NoOfKnapsacks)
                        {
                            foreach (string sss in ss)
                            {
                                if (sss == "//") break;
                                else
                                {
                                    if (sss != "")
                                    {
                                        ret.KnapsackWeights.Add(Convert.ToDouble(sss));
                                        knapsackCapacitiesLineReached++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //last lines
                            if (countItemsWeights < ret.NoOfItems && i < lines.Count() - 1)
                            {
                                foreach (string sss in ss)
                                {
                                    if (sss == "//") break;
                                    else
                                    {
                                        if (sss != "")
                                        {
                                            ret.Items[countItemsWeights].weights.Add(Convert.ToDouble(sss));
                                            countItemsWeights++;
                                            if (countItemsWeights == ret.NoOfItems) countItemsWeights = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (i == lines.Count() - 1)
                {
                    string[] sss = line.Split(' ');
                    foreach (string ssss in sss)
                    {
                        if (ssss != "")
                        {
                            ret.Goal = Convert.ToInt32(ssss);
                            break;
                        }
                    }
                }
                i++;
            }
            return ret;
        }

        public static Knapsack ReadProblemTypeTwo(string filename)
        {
            int i = 0;
            int countMaxWeights = 0;
            Knapsack ret = new Knapsack();
            int countItemsWeights = 0;
            int stage = 0;
            string[] lines = System.IO.File.ReadAllLines(filename);
            foreach (string s_loopVariable in lines)
            {
                string s = s_loopVariable;
                s = s.Trim();
                string[] ss = s.Split(' ');
                if (i == 0)
                {
                    ret.NoOfKnapsacks = Convert.ToInt32(ss[1]);
                    ret.NoOfItems = Convert.ToInt32(ss[0]);
                    if (ss.Length > 2)
                    {
                        ret.Goal = Convert.ToInt32(ss[2]);
                    }
                    i += 1;
                    //first stage
                }
                else if (i == 1)
                {
                    if (ret.Items.Count() < ret.NoOfItems)
                    {
                        foreach (string sss_loopVariable in ss)
                        {
                            string sss = sss_loopVariable;
                            ret.Items.Add(new Item(Convert.ToDouble(sss), new List<double>()));
                        }
                    }
                    else
                    {
                        i += 1;
                    }
                }
                if (i == 2)
                {
                    if (countItemsWeights < ret.NoOfKnapsacks)
                    {
                        for (int index = 0; index <= ss.Length - 1; index++)
                        {
                            ret.Items[stage].weights.Add(Convert.ToDouble(ss[index]));
                            stage += 1;
                        }
                        if (stage == ret.NoOfItems)
                        {
                            countItemsWeights += 1;
                            stage = 0;
                        }

                    }
                    else
                    {
                        i += 1;
                    }
                }
                if (i == 3)
                {
                    if (countMaxWeights < ret.NoOfKnapsacks)
                    {
                        foreach (string sss_loopVariable in ss)
                        {
                            string sss = sss_loopVariable;
                            ret.KnapsackWeights.Add(Convert.ToDouble(sss));
                            countMaxWeights += 1;
                        }
                    }
                    else
                    {
                        i += 1;
                    }
                }
            }
            return ret;
        }

        public Configuration<List<int>> GetConfiguration()
        {
            Configuration<List<int>> config = new Configuration<List<int>>();
            config.CloneFunction = this.Clone;
            config.InitializeSolutionFunction = this.GetInitialSolution;
            config.Movement = Search.Direction.Divergence;
            config.MutationFunction = this.Mutate;
            config.NoOfIterations = 1500;
            config.ObjectiveFunction = this.GetFitness;
            config.PopulationSize = 50;
            config.SelectionFunction = Selection.RoulleteWheel;
            config.WriteToConsole = true;
            config.HardObjectiveFunction = (List<int> sol) =>
            {
                return this.GetFitness(sol) < Double.MaxValue;
            };
            config.EnforceHardObjective = true;
            return config;
        }

        public static List<int> SolveGA(Knapsack k)
        {
            GeneticAlgorithm<List<int>> ga = new GeneticAlgorithm<List<int>>((List<int> sol1, List<int> sol2) => {
                return GA.CrossOver.CutAndSplice<int>(sol1.AsEnumerable(), sol2.AsEnumerable())[0].ToList();
                });
            ga.Create(k.GetConfiguration());
            return ga.FullIteration();
        }

        public static List<int> SolveABC(Knapsack k)
        {
            Hive<List<int>, Bee<List<int>>> hive = new Hive<List<int>, Bee<List<int>>>();
            hive.Create(k.GetConfiguration());
            return hive.FullIteration();
        }

        public static List<int> SolveHC(Knapsack k)
        {
            HillClimb<List<int>> hc = new HillClimb<List<int>>();
            hc.Create(k.GetConfiguration());
            return hc.FullIteration();
        }
    }
}
