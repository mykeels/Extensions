using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace Extensions.Heuristics.Meta.Abc
{
    public partial class Hive<FoodType, IBee> : IMetaHeuristic<FoodType>
    {
        //Note that FoodType and FoodSource should be of Same Class/Type
        private FoodType _bestFood { get; set; } // the best food source
        private double _bestFitness { get; set; } // the quality of the best food source
        private double _acceptProbability = 0.4; // the probability that an onlooker bee will accept a food source proposed by an employee bee
        private Func<FoodType, FoodType> _cloneFunc { get; set; } // a function for cloning a food source. This is important to prevent two bees working on the same object in memory at a time
        private Func<IEnumerable<FoodType>, IEnumerable<double>, int, IEnumerable<FoodType>> _selectionMethod { get; set; }
        public Search.Direction Movement { get; set; }
        public List<IBee<FoodType>> Bees = new List<IBee<FoodType>>();

        private int _iterationCount = 0;
        private List<double> _iterationFitnessSequence = new List<double>();

        public Hive()
        {

        }

        public Hive(Func<FoodType, FoodType> mutateFunc, Func<FoodType, double> fitnessFunc, Func<FoodType, FoodType> cFunc,
           Func<IEnumerable<FoodType>, IEnumerable<double>, int, IEnumerable<FoodType>> selectFunc, int noOfBees = 20,
           int _failureLimit = 20, double _acceptanceProbability = 0.4, Search.Direction _movement = Search.Direction.Optimization)
        {
            this.Create(mutateFunc, fitnessFunc, cFunc, selectFunc, noOfBees, _failureLimit, _acceptanceProbability);
        }

        public void Create(Func<FoodType, FoodType> mutateFunc, Func<FoodType, double> fitnessFunc, Func<FoodType, FoodType> cFunc,
           Func<IEnumerable<FoodType>, IEnumerable<double>, int, IEnumerable<FoodType>> selectFunc)
        {
            this.Create(mutateFunc, fitnessFunc, cFunc, selectFunc, 20);
        }

        public void Create(Func<FoodType, FoodType> mutateFunc, Func<FoodType, double> fitnessFunc, Func<FoodType, FoodType> cFunc,
           Func<IEnumerable<FoodType>, IEnumerable<double>, int, IEnumerable<FoodType>> selectFunc, int noOfBees = 20,
           int _failureLimit = 20, double _acceptanceProbability = 0.4, Search.Direction _movement = Search.Direction.Optimization)
        {
            if (noOfBees <= 1)
            {
                throw new Exception("You dey Craze? Dem tell you say one (1) bee na Swarm?");
            }
            if (Bees.Count == 0)
            {
                for (int index = 1; index <= noOfBees; index++)
                {
                    BeeTypeClass _type = default(BeeTypeClass);
                    if (index < (noOfBees / 2))
                    {
                        _type = BeeTypeClass.Employed;
                    }
                    else
                    {
                        _type = BeeTypeClass.Onlooker;
                    }
                    IBee<FoodType> bee = (IBee<FoodType>)Activator.CreateInstance<IBee>();
                    bee.Init(mutateFunc, fitnessFunc, _type, index - 1, _failureLimit);
                    Bees.Add(bee);
                }
            }
            _acceptProbability = _acceptanceProbability;
            Movement = _movement;
            _cloneFunc = cFunc;
            _selectionMethod = selectFunc;
        }

        private void Start(Func<FoodType> initFunc)
        {
            if (Movement == Search.Direction.Optimization)
            {
                _bestFitness = double.MaxValue;
            }
            else if (Movement == Search.Direction.Divergence)
            {
                _bestFitness = double.MinValue;
            }
            if (Bees.AsEnumerable().Count(_bee => { return _bee.GetFood() != null; }) == 0)
            {
                for (int index = 0; index <= Bees.Count - 1; index++)
                {
                    IBee<FoodType> _bee = this.Bees[index];
                    _bee.SetBeeID(index);
                    if (_bee.GetBeeType() == BeeTypeClass.Employed)
                    {
                        _bee.SetFood(initFunc());
                        _bee.GetFitness();
                    }
                }
            }
        }

        public FoodType SingleIteration(Func<FoodType> initFunc, bool writeToConsole = false)
        {
            if (Bees.All((bee) => bee == default(IBee<FoodType>))) Start(initFunc);
            FoodType ret = default(FoodType);
            IEnumerable<IBee<FoodType>> _employedBees = Bees.Where((IBee<FoodType> _bee) => { return _bee.GetBeeType().Equals(BeeTypeClass.Employed) & _bee.GetFood() != null; }).ToList();
            int _employedCount = _employedBees.Count();
            for (int i = 0; i <= (_employedCount - 1); i++)
            {
                IBee<FoodType> _eBee = _employedBees.ElementAt(i);
                _eBee.SetFood(_eBee.Mutate());
                Bees[_eBee.GetBeeID()] = _eBee;
            }
            this.ShareInformation();
            IEnumerable<double> _fitnesses = Bees.Select((IBee<FoodType> _bee) => { return _bee.GetFitness(); });
            double _bestFit = 0;
            //collate bestFitness and bestFood
            if (Movement == Search.Direction.Divergence)
            {
                _bestFit = _fitnesses.Max();
                ret = Bees[_fitnesses.ToList().IndexOf(_bestFit)].GetFood();
                if (_bestFit > _bestFitness)
                {
                    _bestFitness = _bestFit;
                    _bestFood = _cloneFunc(ret);
                }
            }
            else if (Movement == Search.Direction.Optimization)
            {
                _bestFit = _fitnesses.Min();
                ret = Bees[_fitnesses.ToList().IndexOf(_bestFit)].GetFood();
                if (_bestFit < _bestFitness)
                {
                    _bestFitness = _bestFit;
                    _bestFood = _cloneFunc(ret);
                }
            }
            if (writeToConsole)
            {
                Console.Write(_iterationCount + "\t" + _bestFitness + "\t\t");
                Console.Write("E-Bees: " + _employedCount + '\t');
                Console.Write("On-Bees: " + Convert.ToInt32(Bees.Count - _employedCount) + '\t');
                Console.WriteLine();
            }
            return ret;
        }

        public List<double> GetIterationSequence()
        {
            return _iterationFitnessSequence;
        }

        public FoodType FullIteration(Func<FoodType> initFunc, int noOfIterations = 500, bool writeToConsole = false, Action<FoodType> executeOnBestFood = null)
        {
            Start(initFunc);
            FoodType ret = default(FoodType);
            for (int count = 1; count <= noOfIterations; count++)
            {
                _iterationCount = count;
                ret = SingleIteration(initFunc, writeToConsole && count % 10 == 0);
                _iterationFitnessSequence.Add(_bestFitness);
                executeOnBestFood?.Invoke(ret);
            }
            ret = _bestFood;
            Console.WriteLine("End of Iterations");
            return ret;
        }

        public void ShareInformation()
        {
            IEnumerable<IBee<FoodType>> _employedBees = Bees.Where((IBee<FoodType> _bee) => { return _bee.GetBeeType().Equals(BeeTypeClass.Employed); });
            IEnumerable<IBee<FoodType>> _onlookerBees = Bees.Where((IBee<FoodType> _bee) => { return _bee.GetBeeType().Equals(BeeTypeClass.Onlooker); });
            foreach (IBee<FoodType> _bee in _onlookerBees)
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

        public FoodType SelectFood()
        {
            IEnumerable<IBee<FoodType>> _employedBees = Bees.Where((IBee<FoodType> _bee) => { return _bee.GetBeeType().Equals(BeeTypeClass.Employed) & _bee.GetFood() != null; });
            List<double> fitnesses = _employedBees.Select(_bee => { return _bee.GetFitness(); }).ToList();
            double sum = fitnesses.Sum();
            if (fitnesses.IsEmpty()) return default(FoodType);
            return _selectionMethod(_employedBees.Select((_bee) => _bee.GetFood()), fitnesses, 1).First();
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
