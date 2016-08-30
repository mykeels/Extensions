using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta
{
    public class HillClimb<SolutionType> : IMetaHeuristic<SolutionType>
    {
        private SolutionType _bestIndividual { get; set; }
        private double _bestFitness { get; set; }
        private Func<SolutionType> _initFunc { get; set; }
        private Func<SolutionType, SolutionType> _cloneFunc { get; set; }
        private Func<SolutionType, SolutionType> _mutateFunc { get; set; }
        private Func<SolutionType, double> _fitnessFunc { get; set; }
        public Search.Direction Movement { get; set; }
        private List<double> _iterationFitnessSequence = new List<double>();

        public HillClimb(Search.Direction movement = Search.Direction.Optimization)
        {
            this.Movement = movement;
        }

        public void Create(Func<SolutionType, SolutionType> mutationFunction, Func<SolutionType, double> objectiveFunction, 
            Func<SolutionType, SolutionType> cloneFunction, Func<IEnumerable<SolutionType>, IEnumerable<double>, int, IEnumerable<SolutionType>> selectionFunction = null)
        {
            this._mutateFunc = mutationFunction;
            this._fitnessFunc = objectiveFunction;
            this._cloneFunc = cloneFunction;
        }

        private void Start(Func<SolutionType> initializeSolutionFunction)
        {
            this._initFunc = initializeSolutionFunction;
            if (Movement == Search.Direction.Optimization)
            {
                _bestFitness = double.MaxValue;
            }
            else if (Movement == Search.Direction.Divergence)
            {
                _bestFitness = double.MinValue;
            }
            this._bestIndividual = _initFunc();
            this._bestFitness = _fitnessFunc(this._bestIndividual);
        }

        public SolutionType FullIteration(Func<SolutionType> initializeSolutionFunction, int noOfIterations = 500, bool writeToConsole = false, 
            Action<SolutionType> executeOnBestFood = null)
        {
            this.Start(initializeSolutionFunction);
            for (int count = 1; count <= noOfIterations; count++)
            {
                _bestIndividual = SingleIteration(initializeSolutionFunction, writeToConsole);
                _iterationFitnessSequence.Add(_bestFitness);
                executeOnBestFood?.Invoke(_bestIndividual);
            }
            if (writeToConsole) Console.WriteLine("End of Iterations");
            return _bestIndividual;
        }

        public List<double> GetIterationSequence()
        {
            return _iterationFitnessSequence;
        }

        public SolutionType SingleIteration(Func<SolutionType> initializeSolutionFunction, bool writeToConsole = false)
        {
            if (_bestIndividual == null) this.Start(initializeSolutionFunction);
            SolutionType newSol = _mutateFunc(_cloneFunc(_bestIndividual));
            double newFit = _fitnessFunc(newSol);
            if ((Movement == Search.Direction.Optimization && newFit < _bestFitness) || (Movement == Search.Direction.Divergence && newFit > _bestFitness))
            {
                _bestIndividual = _cloneFunc(newSol);
                _bestFitness = newFit;
            }
            if (writeToConsole) Console.WriteLine(_bestFitness);
            return _bestIndividual;
        }
    }
}
