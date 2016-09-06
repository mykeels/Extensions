using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta
{
    public class SimulatedAnnealing<SolutionType>: IMetaHeuristic<SolutionType>
    {
        public Configuration<SolutionType> Config { get; set; }
        private SolutionType _bestIndividual { get; set; }
        private double _bestFitness { get; set; }
        private int _iterationCount { get; set; }
        private List<double> _iterationFitnessSequence { get; set; }
        private double _initialTemperature { get; set; }
        private double _temperature { get; set; }
        private Func<double, double, double> _acceptanceProbabilityFunction { get; set; }
        private Func<double, double> _temperatureUpdateFunction { get; set; }
        private TemperatureUpdate _temperatureUpdateType { get; set; }

        public SimulatedAnnealing()
        {
            this._initialTemperature = 100;
            this._temperature = 100;
            this._iterationFitnessSequence = new List<double>();
            this._acceptanceProbabilityFunction = defaultAcceptanceProbabilityFunction;
        }

        public SimulatedAnnealing(Func<double, double, double> _acceptanceProbabilityFunction, TemperatureUpdate _temperatureUpdateType, 
            Func<double, double> _temperatureUpdateFunction = null) : this()
        {
            this._acceptanceProbabilityFunction = _acceptanceProbabilityFunction;
            if (_temperatureUpdateType == TemperatureUpdate.Other)
            {
                if (_temperatureUpdateFunction == null) this._temperatureUpdateFunction = defaultTemperatureUpdate;
                else this._temperatureUpdateFunction = _temperatureUpdateFunction;
            }
            else if (_temperatureUpdateType == TemperatureUpdate.Default) this._temperatureUpdateFunction = defaultTemperatureUpdate;
            else if (_temperatureUpdateType == TemperatureUpdate.Boltz) this._temperatureUpdateFunction = boltzTemperatureUpdate;
            else this._temperatureUpdateFunction = fastTemperatureUpdate;
        }

        public void Create(Configuration<SolutionType> config)
        {
            this.Config = config;
            if (_acceptanceProbabilityFunction == null) _acceptanceProbabilityFunction = defaultAcceptanceProbabilityFunction;

        }

        public SolutionType FullIteration()
        {
            throw new NotImplementedException();
        }

        public List<double> GetIterationSequence()
        {
            throw new NotImplementedException();
        }

        public SolutionType SingleIteration()
        {
            throw new NotImplementedException();
        }

        private double defaultAcceptanceProbabilityFunction(double oldFitness, double newFitness)
        {
            if (newFitness < oldFitness) return 1;
            else return 1 / (1 + Math.Exp((newFitness - oldFitness) / _temperature));
        }
        
        public enum TemperatureUpdate
        {
            Default,
            Fast,
            Boltz,
            Other
        }

        private double defaultTemperatureUpdate(double temperature)
        {
            return _initialTemperature * Math.Pow(0.95, _iterationCount);
        }

        private double fastTemperatureUpdate(double temperature)
        {
            return _initialTemperature / _iterationCount;
        }

        private double boltzTemperatureUpdate(double temperature)
        {
            return _initialTemperature / Math.Log(_iterationCount);
        }
    }
}
