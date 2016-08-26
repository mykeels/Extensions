# Meta-Heuristics

In computer science and mathematical optimization, a metaheuristic is a higher-level procedure or heuristic designed to find, generate, or select a heuristic (partial search algorithm) that may provide a sufficiently good solution to an optimization problem, especially with incomplete or imperfect information or ... [see more](https://en.wikipedia.org/wiki/Metaheuristic)

This namespace contains the following:

### IMetaHeuristics

This is an interface. Every meta-heuristic algorithm class in this project implements this interface. It contains methods such as 

```cs
//accepts functions that are neccessary in most meta-heuristic algorithms
void Create(Func<SolutionType, SolutionType> mutationFunction, Func<SolutionType, double> objectiveFunction, Func<SolutionType, SolutionType> cloneFunction, Func<IEnumerable<SolutionType>, IEnumerable<double>, int, IEnumerable<SolutionType>> selectionFunction);

//performs a single iteration step and accepts a function that generates an initial solution
SolutionType SingleIteration(Func<SolutionType> initializeSolutionFunction, bool writeToConsole = false);

//performs a specified number of iterations
SolutionType FullIteration(Func<SolutionType> initializeSolutionFunction, int noOfIterations = 500, bool writeToConsole = false);
```

### LAHC

This means [Late Acceptance Hill Climbing](http://www.yuribykov.com/LAHC/), a meta-heuristic proposed by Yuri Bykov in August 2008. This class has not been implemented as a stand-alone meta-heuristic. 

Rather, a stand-alone meta-heuristic such as [Artificial Bee Colony](https://github.com/mykeels/Extensions/tree/master/Extensions/Heuristics/Meta) can inherit from it, as its Bee class does to create Bee-Lahc, a Bee specie that uses the LAHC algorithm implicitly. 

Such a class would gain late acceptance hill climbing powers, which would give it more exploratory power. Hmm, cool idea for a super power, huh? ExploraBot, the auto bot that goes adventuring in deep space ....

### Search

The Search class is to contain Enums and other common resources to be used throughout this namespace.

## Selection

The Selection class contains static tested selection methods that you can pass into meta-heuristic implementations instead of having to create yours again. You can use selection methods such as:

- Roullete Wheel Selection
This is similar to the russian game with the same name. Spin a wheel containing multiple options, when it stops, the chosen option wins. The probability for an option to be chosen depends on the angle size of its wheel section.

- Rank Based Selection
Here, Solutions are ranked based on their fitness and the best n solutions are selected. 

- Stochastic Universal Sampling Selection
Umm, check it out on the [Wiki](https://en.wikipedia.org/wiki/Stochastic_universal_sampling) i made a contribution to.

- Tournament Selection
Here, a few individuals are selected and made to run a tournament to determine the individuals to be selected. Check out its [Wiki](https://en.wikipedia.org/wiki/Tournament_selection).