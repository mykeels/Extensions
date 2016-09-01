using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta
{
    public interface IMetaHeuristic<SolutionType>
    {
        //void Create(Func<SolutionType, SolutionType> mutationFunction, Func<SolutionType, double> objectiveFunction, Func<SolutionType, SolutionType> cloneFunction,
        //   Func<IEnumerable<SolutionType>, IEnumerable<double>, int, IEnumerable<SolutionType>> selectionFunction);

        void Create(Configuration<SolutionType> config);

        SolutionType SingleIteration();

        SolutionType FullIteration();

        List<double> GetIterationSequence();
    }
}