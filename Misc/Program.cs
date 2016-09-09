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
            Console.WriteLine("<listId>150018</listId>".Between("<listId>", "</listId>"));
            Console.Read();
            Knapsack k = Knapsack.ReadProblemTypeTwo(@"C:\Users\michaeli\Documents\Visual Studio 2015\Projects\Knapsack\Knapsack\bin\Debug\knapsack.txt");
            List<int> bestSol = Knapsack.SolveSA(k);
            Console.Read();
            //Console.WriteLine("DKTBXS5sj86VoDP8Pz27eg==".Decrypt());
            //byte[] food = EightQueens.GetCorrectSolutionSA();
            //Console.WriteLine(food.Join(",") + " = " + EightQueens.GetSolutionFitness(food) + " queens check each other");
            //Console.Read();
        }
    }
}
