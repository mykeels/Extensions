using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta
{
    public interface IMetaHeuristic<SolutionType>
    {
        void Create(Func<SolutionType, SolutionType> mutationFunction, Func<SolutionType, double> objectiveFunction, Func<SolutionType, SolutionType> cloneFunction,
           Func<IEnumerable<SolutionType>, IEnumerable<double>, int, IEnumerable<SolutionType>> selectionFunction);

        SolutionType SingleIteration(Func<SolutionType> initializeSolutionFunction, bool writeToConsole = false);

        SolutionType FullIteration(Func<SolutionType> initializeSolutionFunction, int noOfIterations = 500, bool writeToConsole = false, Action<SolutionType> executeOnBestFood = null);

        List<double> GetIterationSequence();
    }
}
