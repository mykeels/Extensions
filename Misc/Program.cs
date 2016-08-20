using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using Extensions.Models;
using Extensions.Heuristics.Meta;
using Extensions.Heuristics.Meta.Abc;
using Extensions.Heuristics.Meta.Problems;

namespace Misc
{
    public class Program
    {
        public static void Main(String[] args)
        {
            byte[] food = EightQueens.GetCorrectSolutionGA(true);
            Console.WriteLine(food.Join(",") + " = " + EightQueens.GetSolutionFitness(food) + " queens check each other");
            Console.Read();
        }

        
    }
}
