using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta
{
    public class Configuration<SolutionType>
    {
        public Func<SolutionType, SolutionType> MutationFunction { get; set; }
        public Func<SolutionType, double> ObjectiveFunction { get; set; }
        public Func<SolutionType, SolutionType> CloneFunction { get; set; }
        public Func<IEnumerable<SolutionType>, IEnumerable<double>, int, IEnumerable<SolutionType>> SelectionFunction { get; set; }
        public Func<SolutionType> InitializeSolutionFunction { get; set; }
        public Func<SolutionType, bool> HardObjectiveFunction { get; set; }
        public Action<SolutionType, double, int> ConsoleWriteFunction { get; set; }
        public bool EnforceHardObjective { get; set; }
        public bool WriteToConsole { get; set; }
        public int ConsoleWriteInterval { get; set; }
        public int NoOfIterations { get; set; }
        public int PopulationSize { get; set; }
        public Search.Direction Movement { get; set; }

        public Configuration()
        {
            this.NoOfIterations = 500;
            this.WriteToConsole = true;
            this.EnforceHardObjective = false;
            this.Movement = Search.Direction.Optimization;
            this.PopulationSize = 1;
            this.ConsoleWriteInterval = 10;
        }
        
        public bool newFitnessIsBetter(double oldFitness, double newFitness)
        {
            return (Movement == Search.Direction.Optimization && newFitness < oldFitness) ||
                (Movement == Search.Direction.Divergence && newFitness > oldFitness);
        }
    }
}
