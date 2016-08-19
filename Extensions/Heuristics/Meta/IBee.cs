using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta
{
    public enum BeeTypeClass
    {
        Employed,
        Onlooker,
        Scout
    }

    public interface IBee<FoodSource>
    {
        void Init(Func<FoodSource, FoodSource> mFunc, Func<FoodSource, double> fFunc, BeeTypeClass _type, int ID = 0, int _failureLimit = 20);
        void ChangeToEmployed(FoodSource _food, Func<FoodSource, FoodSource> mFunc = null, Func<FoodSource, double> fFunc = null);
        void ChangeToOnlooker();
        void ChangeToScout();
        FoodSource Mutate();
        double GetFitness();
        BeeTypeClass GetBeeType();
        FoodSource GetFood();
        FoodSource SetFood(FoodSource food);
        int GetBeeID();
        int SetBeeID(int ID);
    }
}
