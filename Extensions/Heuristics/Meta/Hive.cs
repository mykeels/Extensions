using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace Extensions.Heuristics.Meta
{
    public class Hive<FoodType>
    {
        //Note that FoodType and FoodSource should be of Same Class/Type
        private static FoodType _bestFood { get; set; }
        private static double _bestFitness { get; set; }
        private static Func<FoodType, FoodType> _cloneFunc { get; set; }
        public static List<Bee<FoodType>> Bees { get; set; }
        public static Search.Direction Movement { get; set; }
        
        public static void Create(Func<FoodType, FoodType> mutateFunc, Func<FoodType, double> fitnessFunc, Func<FoodType, FoodType> cFunc, 
            int noOfBees = 20, int _failureLimit = 20, Search.Direction _movement = Search.Direction.Optimization)
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
            Movement = _movement;
            _cloneFunc = cFunc;
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

        public static Bee<FoodType> SingleIteration(Func<FoodType> initFunc)
        {
            //'Start Algorithm
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
            Console.WriteLine(_bestFitness);
            return ret;
        }

        public static void FullIteration(Func<FoodType> initFunc, int noOfIterations = 500)
        {
            Start(initFunc);
            for (int count = 1; count <= noOfIterations; count++)
            {
                SingleIteration(initFunc);
            }
        }

        public static void ShareInformation()
        {
            IEnumerable<Bee<FoodType>> _employedBees = Bees.Where((Bee<FoodType> _bee) => { return _bee.Type.Equals(Bee<FoodType>.TypeClass.Employed) & _bee.Food != null; });
            IEnumerable<Bee<FoodType>> _onlookerBees = Bees.Where((Bee<FoodType> _bee) => { return _bee.Type.Equals(Bee<FoodType>.TypeClass.Onlooker); });
            foreach (Bee<FoodType> _bee in _onlookerBees)
            {
                FoodType _food = SelectFood();
                if (_food != null)
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
            List<double> fitnesses = _employedBees.Select(_bee => { return _bee.Fitness; }).ToList();
            double sum = fitnesses.Sum();
            int selectedIndex = 0;
            if (fitnesses.IsEmpty()) return default(FoodType);
            while (true)
            {
                foreach (double fitness in fitnesses)
                {
                    double probability = fitness / sum;
                    if (Number.Rnd() < probability)
                    {
                        return _cloneFunc(_employedBees.ElementAt(selectedIndex).Food);
                    }
                    selectedIndex += 1;
                }
            }
        }

        public class Bee<FoodSource>
        {
            public int ID { get; set; }
            public TypeClass Type { get; set; }
            public FoodSource Food { get; set; }
            public double Fitness { get; set; }
            public double defaultFitness { get; set; }
            private int _timeSinceLastImprovement { get; set; }
            private int _nonImprovementLimit { get; set; }
            private Func<FoodSource, FoodSource> _mutationFunc { get; set; }
            private Func<FoodSource, double> _fitnessFunc { get; set; }

            public Bee(Func<FoodSource, FoodSource> mFunc, Func<FoodSource, double> fFunc, TypeClass _type, int ID = 0, int _failureLimit = 20)
            {
                if ((Hive<FoodSource>.Movement == Search.Direction.Divergence))
                {
                    this.Fitness = double.MinValue;
                    defaultFitness = double.MinValue;
                }
                else if (Hive<FoodSource>.Movement == Search.Direction.Optimization)
                {
                    this.Fitness = double.MaxValue;
                    defaultFitness = double.MaxValue;
                }
                if (mFunc == null | fFunc == null)
                {
                    throw new Exception(string.Format("Ogbeni, na Bee #{0} be this. How i wan take mutate na?", ID));
                }
                this._nonImprovementLimit = _failureLimit;
                this._mutationFunc = mFunc;
                this._fitnessFunc = fFunc;
                this.Type = _type;
            }

            public void ChangeToEmployed(FoodSource _food, Func<FoodSource, FoodSource> mFunc = null, Func<FoodSource, double> fFunc = null)
            {
                this.Type = Hive<FoodType>.Bee<FoodSource>.TypeClass.Employed;
                this.Food = _food;
                this.GetFitness();
                if (mFunc != null)
                {
                    this._mutationFunc = mFunc;
                }
                if (fFunc != null)
                {
                    this._fitnessFunc = fFunc;
                }
            }

            public void ChangeToOnlooker()
            {
                this.Type = Hive<FoodType>.Bee<FoodSource>.TypeClass.Onlooker;
                this.Food = default(FoodSource);
                this.Fitness = defaultFitness;
            }
            
            public void ChangeToScout()
            {
                this.Type = Hive<FoodType>.Bee<FoodSource>.TypeClass.Scout;
                this.Food = default(FoodSource);
                this.Fitness = defaultFitness;
            }

            #region "EmployedBees"
            public FoodSource Mutate()
            {
                FoodSource ret = _mutationFunc(this.Food);
                double _fitness = Bee<FoodSource>.GetFitness(ret, _fitnessFunc);
                if (this.Fitness.Equals(defaultFitness) | _fitness < this.Fitness)
                {
                    this.Fitness = _fitness;
                    this.Food = ret;
                    this._timeSinceLastImprovement = 0;
                }
                else
                {
                    this._timeSinceLastImprovement += 1;
                    //Solution has not improved
                    if (this._timeSinceLastImprovement >= this._nonImprovementLimit)
                    {
                        this.ChangeToOnlooker();
                    }
                }
                return this.Food;
            }

            public double GetFitness()
            {
                this.Fitness = _fitnessFunc(this.Food);
                return this.Fitness;
            }
            #endregion

            public static double GetFitness(FoodSource _food, Func<FoodSource, double> _fitnessFunc)
            {
                return _fitnessFunc(_food);
            }

            #region "OnlookerBees"

            #endregion

            public enum TypeClass
            {
                Employed,
                Onlooker,
                Scout
            }
        }

    }
}
