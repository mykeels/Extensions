using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace Extensions.Heuristics.Meta
{
    public partial class Hive<FoodType>
    {
        //Note that FoodType and FoodSource should be of Same Class/Type
        private static FoodType _bestFood { get; set; } // the best food source
        private static double _bestFitness { get; set; } // the quality of the best food source
        private static double _acceptProbability = 0.4; // the probability that an onlooker bee will accept a food source proposed by an employee bee
        private static Func<FoodType, FoodType> _cloneFunc { get; set; } // a function for cloning a food source. This is important to prevent two bees working on the same object in memory at a time
        private static Func<IEnumerable<FoodType>, IEnumerable<double>, int, IEnumerable<FoodType>> _selectionMethod { get; set; }
        public static Search.Direction Movement { get; set; }

        public static List<Bee<FoodType>> Bees = new List<Bee<FoodType>>();

        public static void Create(Func<FoodType, FoodType> mutateFunc, Func<FoodType, double> fitnessFunc, Func<FoodType, FoodType> cFunc,
           Func<IEnumerable<FoodType>, IEnumerable<double>, int, IEnumerable<FoodType>> selectFunc, int noOfBees = 20, 
           int _failureLimit = 20, double _acceptanceProbability = 0.4,  Search.Direction _movement = Search.Direction.Optimization)
        {
            if (noOfBees <= 1)
            {
                throw new Exception("You dey Craze? Dem tell you say one (1) bee na Swarm?");
            }
            if (Bees.Count == 0)
            {
                for (int index = 1; index <= noOfBees; index++)
                {
                    Bee<FoodType>.TypeClass _type = default(Bee<FoodType>.TypeClass);
                    if (index < (noOfBees / 2))
                    {
                        _type = Hive<FoodType>.Bee<FoodType>.TypeClass.Employed;
                    }
                    else
                    {
                        _type = Hive<FoodType>.Bee<FoodType>.TypeClass.Onlooker;
                    }
                    Bees.Add(new Bee<FoodType>(mutateFunc, fitnessFunc, _type, index - 1, _failureLimit));
                }
            }
            _acceptProbability = _acceptanceProbability;
            Movement = _movement;
            _cloneFunc = cFunc;
            _selectionMethod = selectFunc;
        }

        private static void Start(Func<FoodType> initFunc)
        {
            if (Movement == Search.Direction.Optimization)
            {
                _bestFitness = double.MaxValue;
            }
            else if (Movement == Search.Direction.Divergence)
            {
                _bestFitness = double.MinValue;
            }
            if (Bees.AsEnumerable().Count(_bee => { return _bee.Food != null; }) == 0)
            {
                for (int index = 0; index <= Bees.Count - 1; index++)
                {
                    Bee<FoodType> _bee = Hive<FoodType>.Bees[index];
                    _bee.ID = index;
                    if (_bee.Type == Hive<FoodType>.Bee<FoodType>.TypeClass.Employed)
                    {
                        _bee.Food = initFunc();
                        _bee.GetFitness();
                    }
                }
            }
        }

        public static Bee<FoodType> SingleIteration(Func<FoodType> initFunc, bool writeToConsole = false)
        {
            //Start Algorithm
            Bee<FoodType> ret = null;
            IEnumerable<Bee<FoodType>> _employedBees = Bees.Where((Bee<FoodType> _bee) => { return _bee.Type.Equals(Bee<FoodType>.TypeClass.Employed) & _bee.Food != null; }).ToList();
            int _employedCount = _employedBees.Count();
            for (int i = 0; i <= (_employedCount - 1); i++)
            {
                Bee<FoodType> _eBee = _employedBees.ElementAt(i);
                _eBee.Mutate();
                Bees[_eBee.ID] = _eBee;
            }
            Hive<FoodType>.ShareInformation();
            IEnumerable<double> _fitnesses = Bees.Select((Bee<FoodType> _bee) => { return _bee.Fitness; });
            double _bestFit = 0;
            //collate bestFitness and bestFood
            if (Movement == Search.Direction.Divergence)
            {
                _bestFit = _fitnesses.Max();
                ret = Bees[_fitnesses.ToList().IndexOf(_bestFit)];
                if (_bestFit > _bestFitness)
                {
                    _bestFitness = _bestFit;
                    _bestFood = ret.Food;
                }
            }
            else if (Movement == Search.Direction.Optimization)
            {
                _bestFit = _fitnesses.Min();
                ret = Bees[_fitnesses.ToList().IndexOf(_bestFit)];
                if (_bestFit < _bestFitness)
                {
                    _bestFitness = _bestFit;
                    _bestFood = ret.Food;
                }
            }
            if (writeToConsole) {
                Console.Write(_bestFitness + "\t\t");
                Console.Write("E-Bees: " + _employedCount + '\t');
                Console.Write("On-Bees: " + Convert.ToInt32(Bees.Count - _employedCount) + '\t');
                Console.WriteLine();
            }
            return ret;
        }

        public static Bee<FoodType> FullIteration(Func<FoodType> initFunc, int noOfIterations = 500, bool writeToConsole = false)
        {
            Start(initFunc);
            Bee<FoodType> ret = null;
            for (int count = 1; count <= noOfIterations; count++)
            {
                ret = SingleIteration(initFunc, writeToConsole);
            }
            Console.WriteLine("End of Iterations");
            return ret;
        }

        public static void ShareInformation()
        {
            IEnumerable<Bee<FoodType>> _employedBees = Bees.Where((Bee<FoodType> _bee) => { return _bee.Type.Equals(Bee<FoodType>.TypeClass.Employed) & _bee.Food != null; });
            IEnumerable<Bee<FoodType>> _onlookerBees = Bees.Where((Bee<FoodType> _bee) => { return _bee.Type.Equals(Bee<FoodType>.TypeClass.Onlooker); });
            foreach (Bee<FoodType> _bee in _onlookerBees)
            {
                FoodType _food = SelectFood();
                if (_food != null && Number.Rnd() < _acceptProbability)
                {
                    _bee.ChangeToEmployed(_food);
                }
            }
            if (_employedBees.Count() == 0 & _onlookerBees.Count() > 0)
            {
                _onlookerBees.First().ChangeToEmployed(_bestFood);
            }
        }

        public static FoodType SelectFood()
        {
            IEnumerable<Bee<FoodType>> _employedBees = Bees.Where((Bee<FoodType> _bee) => { return _bee.Type.Equals(Bee<FoodType>.TypeClass.Employed) & _bee.Food != null; });
            List<double> fitnesses = _employedBees.Select(_bee => { return _bee.GetFitness(); }).ToList();
            double sum = fitnesses.Sum();
            if (fitnesses.IsEmpty()) return default(FoodType);
            return _selectionMethod(_employedBees.Select((_bee) => _bee.Food), fitnesses, 1).First();
            /*while (true)
            {
                int selectedIndex = 0;
                foreach (double fitness in fitnesses)
                {
                    double probability = fitness / sum;
                    if (Number.Rnd() < probability)
                    {
                        return _cloneFunc(_employedBees.ElementAt(selectedIndex).Food);
                    }
                    selectedIndex += 1;
                }
            }*/
        }
    }
}
