using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta
{
    public class HillClimb<SolutionType> : IMetaHeuristic<SolutionType>
    {
        public Configuration<SolutionType> Config { get; set; }
        private SolutionType _bestIndividual { get; set; }
        private double _bestFitness { get; set; }
        private int _iterationCount = 0;
        private List<double> _iterationFitnessSequence = new List<double>();

        public HillClimb()
        {

        }

        public void Create(Configuration<SolutionType> config)
        {
            this.Config = config;
            if (Config.Movement == Search.Direction.Optimization)
            {
                _bestFitness = double.MaxValue;
            }
            else if (Config.Movement == Search.Direction.Divergence)
            {
                _bestFitness = double.MinValue;
            }
            this._bestIndividual = Config.InitializeSolutionFunction();
            this._bestFitness = Config.ObjectiveFunction(this._bestIndividual);
        }

        public SolutionType FullIteration()
        {
            for (int count = 1; count <= Config.NoOfIterations; count++)
            {
                _iterationCount = count;
                _bestIndividual = SingleIteration();
                _iterationFitnessSequence.Add(_bestFitness);
            }
            if (Config.WriteToConsole) Console.WriteLine("End of Iterations");
            return _bestIndividual;
        }

        public List<double> GetIterationSequence()
        {
            return _iterationFitnessSequence;
        }

        public SolutionType SingleIteration()
        {
            SolutionType newSol = Config.MutationFunction(Config.CloneFunction(_bestIndividual));
            double newFit = Config.ObjectiveFunction(newSol);

            if ((Config.HardObjectiveFunction != null &&
                    ((Config.EnforceHardObjective && Config.HardObjectiveFunction(newSol)) || (!Config.EnforceHardObjective))) ||
                    Config.HardObjectiveFunction == null)
            {
                if ((Config.Movement == Search.Direction.Optimization && newFit < _bestFitness) || (Config.Movement == Search.Direction.Divergence && newFit > _bestFitness))
                {
                    _bestIndividual = Config.CloneFunction(newSol);
                    _bestFitness = newFit;
                }
            }   
            if (Config.WriteToConsole && _iterationCount % Config.ConsoleWriteInterval == 0) Console.WriteLine(_iterationCount + "\t" + _bestIndividual.ToJson() + " = " + _bestFitness);
            return _bestIndividual;
        }
    }
}
