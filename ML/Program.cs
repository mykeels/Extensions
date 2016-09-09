using System;
using Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl;
using numl.Model;
using numl.Supervised.DecisionTree;

namespace ML
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Tennis[] data = Tennis.GetData();
            var d = Descriptor.Create<Tennis>();
            var g = new DecisionTreeGenerator(d);
            g.SetHint(false);
            var model = Learner.Learn(data, 0.80, 1000, g);
            Console.WriteLine(model.Model.Predict(new Tennis()
            {
                Outlook = Outlook.Overcast,
                Temperature = Temperature.Low,
                Windy = true
            }).ToJson(true));
            Console.Read();
        }

        public enum Outlook
        {
            Sunny,
            Overcast,
            Rainy
        }

        public enum Temperature
        {
            Low,
            High
        }

        public class Tennis
        {
            [Feature]
            public Outlook Outlook { get; set; }
            [Feature]
            public Temperature Temperature { get; set; }
            [Feature]
            public bool Windy { get; set; }
            [Label]
            public bool Play { get; set; }

            public static Tennis[] GetData()
            {
                return new Tennis[]  {
                new Tennis { Play = true, Outlook=Outlook.Sunny, Temperature = Temperature.Low, Windy=true},
                new Tennis { Play = false, Outlook=Outlook.Sunny, Temperature = Temperature.High, Windy=true},
                new Tennis { Play = false, Outlook=Outlook.Sunny, Temperature = Temperature.High, Windy=false},
                new Tennis { Play = true, Outlook=Outlook.Overcast, Temperature = Temperature.Low, Windy=true},
                new Tennis { Play = true, Outlook=Outlook.Overcast, Temperature = Temperature.High, Windy= false},
                new Tennis { Play = true, Outlook=Outlook.Overcast, Temperature = Temperature.Low, Windy=false},
                new Tennis { Play = false, Outlook=Outlook.Rainy, Temperature = Temperature.Low, Windy=true},
                new Tennis { Play = true, Outlook=Outlook.Rainy, Temperature = Temperature.Low, Windy=false}
            };
            }
        }
    }
}