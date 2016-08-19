using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using Extensions.Models;
using Extensions.Heuristics.Meta;

namespace Misc
{
    public class Program
    {
        public static void Main(String[] args)
        {
            byte[] food = EightQueens.GetCorrectSolution();
            Console.WriteLine(food.Join(",") + " = " + EightQueens.GetSolutionFitness(food) + " queens check each other");
            Console.Read();
        }

        
    }
}
