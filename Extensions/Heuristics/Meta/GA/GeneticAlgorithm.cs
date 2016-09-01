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
        public Configuration<IndividualType> Config { get; set; }
        private IndividualType _bestIndividual { get; set; }
        private double _bestFitness { get; set; }
        private int _iterationCount { get; set; }
        //private Func<IndividualType> _initFunc { get; set; }
        private Func<IndividualType, IndividualType, IndividualType> _crossOverFunc { get; set; }
        public List<KeyValue<IndividualType, double>> Population { get; set; }
        private List<double> _iterationFitnessSequence = new List<double>();

        public GeneticAlgorithm()
        {

        }

        public GeneticAlgorithm(Func<IndividualType, IndividualType, IndividualType> crossOverFunction)
        {
            this._crossOverFunc = crossOverFunction;
        }

        public void Create(Configuration<IndividualType> config)
        {
            this.Config = config;
            this.Population = new List<KeyValue<IndividualType, double>>();
            if (config.PopulationSize <= 2) throw new Exception("Population Size must be more than 2");
            if (_crossOverFunc == null) throw new Exception("Please provide a Cross Over Function");
            for (int i = 0; i < config.PopulationSize; i++)
            {
                this.Population.Add(new KeyValue<IndividualType, double>());
            }
            if (config.Movement == Search.Direction.Optimization)
            {
                _bestFitness = double.MaxValue;
            }
            else if (config.Movement == Search.Direction.Divergence)
            {
                _bestFitness = double.MinValue;
            }
            for (int index = 0; index <= Population.Count - 1; index++)
            {
                Population[index].key = config.InitializeSolutionFunction();
                Population[index].value = config.ObjectiveFunction.Invoke(Population[index].key);
            }
        }

        public List<double> GetIterationSequence()
        {
            return _iterationFitnessSequence;
        }

        public IndividualType FullIteration()
        {
            for (int count = 1; count <= Config.NoOfIterations; count++)
            {
                _iterationCount = count;
                IndividualType _bestIndividual = SingleIteration();
                _iterationFitnessSequence.Add(_bestFitness);
            }
            Console.WriteLine("End of Iterations");
            return _bestIndividual;
        }

        public IndividualType SingleIteration()
        {
            var individuals = Config.SelectionFunction.Invoke(Population.Select((individual) => individual.key), Population.Select((individual) => individual.value), 2);
            IndividualType individualA = individuals.ElementAt(0);
            IndividualType individualB = individuals.ElementAt(1);
            double fitnessA = Config.ObjectiveFunction.Invoke(individualA);
            double fitnessB = Config.ObjectiveFunction.Invoke(individualB);
            int indexA = Population.Select((individual) => individual.value).ToList().IndexOf(fitnessA);
            int indexB = Population.Select((individual) => individual.value).ToList().IndexOf(fitnessB);

            //cross-over
            IndividualType newIndividual = this._crossOverFunc(Config.CloneFunction.Invoke(individualA), Config.CloneFunction.Invoke(individualB));
            double newFitness = Config.ObjectiveFunction.Invoke(newIndividual);



            if ((Config.HardObjectiveFunction != null &&
                    ((Config.EnforceHardObjective && Config.HardObjectiveFunction(newIndividual)) || (!Config.EnforceHardObjective))) ||
                    Config.HardObjectiveFunction == null)
            {

                if ((Config.Movement == Search.Direction.Optimization && newFitness < _bestFitness) ||
                (Config.Movement == Search.Direction.Divergence && newFitness > _bestFitness))
                {

                    _bestIndividual = newIndividual;
                    _bestFitness = newFitness;
                }

                if ((Config.Movement == Search.Direction.Optimization && newFitness < fitnessA) ||
                        (Config.Movement == Search.Direction.Divergence && newFitness > fitnessA))
                {
                    individualA = newIndividual;
                    Population[indexA].key = individualA;
                    Population[indexA].value = newFitness;
                    fitnessA = newFitness;
                }
                else if ((Config.Movement == Search.Direction.Optimization && newFitness < fitnessB) ||
                    (Config.Movement == Search.Direction.Divergence && newFitness > fitnessB))
                {
                    individualB = newIndividual;
                    Population[indexA].key = individualB;
                    Population[indexA].value = newFitness;
                    fitnessB = newFitness;
                }
            }

            //mutation
            line1:
            IndividualType individualAClone = Config.MutationFunction.Invoke(Config.CloneFunction.Invoke(individualA));
            IndividualType individualBClone = Config.MutationFunction.Invoke(Config.CloneFunction.Invoke(individualB));
            double fitnessAClone = Config.ObjectiveFunction.Invoke(individualAClone);
            double fitnessBClone = Config.ObjectiveFunction.Invoke(individualBClone);

            if (Config.HardObjectiveFunction != null &&
                    ((Config.EnforceHardObjective &&
                    !Config.HardObjectiveFunction(individualAClone) || !Config.HardObjectiveFunction(individualBClone))))
            {
                goto line1;
            }

            if ((Config.Movement == Search.Direction.Optimization && fitnessAClone < _bestFitness) ||
                (Config.Movement == Search.Direction.Divergence && fitnessAClone > _bestFitness))
            {
                _bestIndividual = individualAClone;
                _bestFitness = fitnessAClone;
            }
            else if ((Config.Movement == Search.Direction.Optimization && fitnessBClone < _bestFitness) ||
                (Config.Movement == Search.Direction.Divergence && fitnessBClone > _bestFitness))
            {
                _bestIndividual = individualBClone;
                _bestFitness = fitnessBClone;
            }

            if ((Config.Movement == Search.Direction.Optimization && fitnessAClone < fitnessA) ||
                (Config.Movement == Search.Direction.Divergence && fitnessAClone > fitnessA))
            {
                Population[indexA].key = individualAClone;
                Population[indexA].value = fitnessAClone;
            }
            else if ((Config.Movement == Search.Direction.Optimization && fitnessAClone < fitnessB) ||
                (Config.Movement == Search.Direction.Divergence && fitnessAClone > fitnessB))
            {
                Population[indexB].key = individualAClone;
                Population[indexB].value = fitnessAClone;
            }
            else if ((Config.Movement == Search.Direction.Optimization && fitnessBClone < fitnessA) ||
                (Config.Movement == Search.Direction.Divergence && fitnessBClone > fitnessA))
            {
                Population[indexA].key = individualBClone;
                Population[indexA].value = fitnessBClone;
            }
            else if ((Config.Movement == Search.Direction.Optimization && fitnessBClone < fitnessB) ||
                (Config.Movement == Search.Direction.Divergence && fitnessBClone > fitnessB))
            {
                Population[indexB].key = individualBClone;
                Population[indexB].value = fitnessBClone;
            }
            if (Config.WriteToConsole && _iterationCount % Config.ConsoleWriteInterval == 0)
            {
                Console.WriteLine(_iterationCount + "\t" + _bestIndividual.ToJson() + " = " + _bestFitness);
            }
            return _bestIndividual;
        }
    }
}
