using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.Models;

namespace Extensions.Heuristics.Meta.GA
{
    public class GeneticAlgorithm<IndividualType> : IMetaHeuristic<IndividualType>
    {
        private IndividualType _bestIndividual { get; set; }
        private double _bestFitness { get; set; }
        private Func<IndividualType> _initFunc { get; set; }
        private Func<IndividualType, IndividualType, IndividualType> _crossOverFunc { get; set; }
        private Func<IndividualType, IndividualType> _cloneFunc { get; set; }
        private Func<IndividualType, IndividualType> _mutateFunc { get; set; }
        private Func<IndividualType, double> _fitnessFunc { get; set; }
        private Func<IEnumerable<IndividualType>, IEnumerable<double>, int, IEnumerable<IndividualType>> _selectionMethod { get; set; }
        public List<KeyValue<IndividualType, double>> Population { get; set; }
        public int PopulationSize { get; set; }
        public Search.Direction Movement { get; set; }

        public GeneticAlgorithm()
        {
            this.Init();
        }

        public GeneticAlgorithm(int populationSize, Func<IndividualType, IndividualType, IndividualType> crossOverFunction, 
            Search.Direction movement = Search.Direction.Optimization)
        {
            this._crossOverFunc = crossOverFunction;
            this.PopulationSize = populationSize;
            this.Movement = movement;
            this.Init();
        }

        private void Init()
        {
            this.Population = new List<KeyValue<IndividualType, double>>();
        }

        public void Create(Func<IndividualType, IndividualType> mutationFunction, Func<IndividualType, double> objectiveFunction,
            Func<IndividualType, IndividualType> cloneFunction,
            Func<IEnumerable<IndividualType>, IEnumerable<double>, int, IEnumerable<IndividualType>> selectionFunction)
        {
            if (PopulationSize <= 2) throw new Exception("Population Size must be more than 2");
            this._mutateFunc = mutationFunction;
            this._fitnessFunc = objectiveFunction;
            this._cloneFunc = cloneFunction;
            this._selectionMethod = selectionFunction;
            for (int i = 0; i < PopulationSize; i++)
            {
                this.Population.Add(new KeyValue<IndividualType, double>());
            }
        }

        private void Start(Func<IndividualType> initializeFunction)
        {
            this._initFunc = initializeFunction;
            if (Movement == Search.Direction.Optimization)
            {
                _bestFitness = double.MaxValue;
            }
            else if (Movement == Search.Direction.Divergence)
            {
                _bestFitness = double.MinValue;
            }
            for (int index = 0; index <= Population.Count - 1; index++)
            {
                Population[index].key = this._initFunc();
                Population[index].value = this._fitnessFunc(Population[index].key);
            }
        }

        public IndividualType FullIteration(Func<IndividualType> initializeSolutionFunction, int noOfIterations = 500, bool writeToConsole = false)
        {
            Start(initializeSolutionFunction);
            for (int count = 1; count <= noOfIterations; count++)
            {
                SingleIteration(initializeSolutionFunction, writeToConsole);
            }
            Console.WriteLine("End of Iterations");
            return _bestIndividual;
        }

        public IndividualType SingleIteration(Func<IndividualType> initializeSolutionFunction, bool writeToConsole = false)
        {
            if (Population.All((individual) => individual == null)) Start(initializeSolutionFunction);
            var individuals = _selectionMethod(Population.Select((individual) => individual.key), Population.Select((individual) => individual.value), 2);
            IndividualType individualA = individuals.ElementAt(0);
            IndividualType individualB = individuals.ElementAt(1);
            double fitnessA = _fitnessFunc(individualA);
            double fitnessB = _fitnessFunc(individualB);
            int indexA = Population.Select((individual) => individual.value).ToList().IndexOf(fitnessA);
            int indexB = Population.Select((individual) => individual.value).ToList().IndexOf(fitnessB);

            //cross-over
            IndividualType newIndividual = this._crossOverFunc(_cloneFunc(individualA), _cloneFunc(individualB));
            double newFitness = _fitnessFunc(newIndividual);

            if (newFitness < _bestFitness)
            {
                _bestIndividual = newIndividual;
                _bestFitness = newFitness;
            }

            if ((Movement == Search.Direction.Optimization && newFitness < fitnessA) ||
                    (Movement == Search.Direction.Divergence && newFitness > fitnessA))
            {
                individualA = newIndividual;
                Population[indexA].key = individualA;
                Population[indexA].value = newFitness;
                fitnessA = newFitness;
            }
            else if ((Movement == Search.Direction.Optimization && newFitness < fitnessB) ||
                (Movement == Search.Direction.Divergence && newFitness > fitnessB))
            {
                individualB = newIndividual;
                Population[indexA].key = individualB;
                Population[indexA].value = newFitness;
                fitnessB = newFitness;
            }

            //mutation
            IndividualType individualAClone = _mutateFunc(_cloneFunc(individualA));
            IndividualType individualBClone = _mutateFunc(_cloneFunc(individualB));
            double fitnessAClone = _fitnessFunc(individualAClone);
            double fitnessBClone = _fitnessFunc(individualBClone);

            if (fitnessAClone < _bestFitness)
            {
                _bestIndividual = individualAClone;
                _bestFitness = fitnessAClone;
            }
            else if (fitnessAClone < _bestFitness)
            {
                _bestIndividual = individualAClone;
                _bestFitness = fitnessBClone;
            }

            if ((Movement == Search.Direction.Optimization && fitnessAClone < fitnessA) ||
                (Movement == Search.Direction.Divergence && fitnessAClone > fitnessA))
            {
                Population[indexA].key = individualAClone;
                Population[indexA].value = fitnessAClone;
            }
            else if ((Movement == Search.Direction.Optimization && fitnessAClone < fitnessB) ||
                (Movement == Search.Direction.Divergence && fitnessAClone > fitnessB))
            {
                Population[indexB].key = individualAClone;
                Population[indexB].value = fitnessAClone;
            }
            else if ((Movement == Search.Direction.Optimization && fitnessBClone < fitnessA) ||
                (Movement == Search.Direction.Divergence && fitnessBClone > fitnessA))
            {
                Population[indexA].key = individualBClone;
                Population[indexA].value = fitnessBClone;
            }
            else if ((Movement == Search.Direction.Optimization && fitnessBClone < fitnessB) ||
                (Movement == Search.Direction.Divergence && fitnessBClone > fitnessB))
            {
                Population[indexB].key = individualBClone;
                Population[indexB].value = fitnessBClone;
            }
            if (writeToConsole)
            {
                Console.WriteLine(_bestIndividual.ToJson() + " = " + _bestFitness);
            }
            return _bestIndividual;
        }
    }
}
