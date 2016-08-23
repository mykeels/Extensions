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
        public class Bee<FoodSource>
        {
            public int ID { get; set; }
            public TypeClass Type { get; set; }
            public FoodSource Food { get; set; }
            public double Fitness { get; set; }
            public double defaultFitness { get; set; }
            protected virtual int _timeSinceLastImprovement { get; set; }
            protected virtual int _nonImprovementLimit { get; set; }
            protected virtual Func<FoodSource, FoodSource> _mutationFunc { get; set; }
            protected virtual Func<FoodSource, double> _fitnessFunc { get; set; }

            public Bee()
            {
                this.Fitness = double.MaxValue;
                this.defaultFitness = double.MaxValue;
                this.Type = TypeClass.Scout;
            }

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
                this._timeSinceLastImprovement = 0;
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
                this._timeSinceLastImprovement = 0;
            }

            public void ChangeToScout()
            {
                this.Type = Hive<FoodType>.Bee<FoodSource>.TypeClass.Scout;
                this.Food = default(FoodSource);
                this.Fitness = defaultFitness;
            }

            #region "EmployedBees"
            public virtual FoodSource Mutate()
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
