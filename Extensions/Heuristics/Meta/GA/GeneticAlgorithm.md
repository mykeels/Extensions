# Genetic Algorithm

This is an evolutionary algorithm, a meta-heuristic inspired by the process of natural selection. It is perhaps the most popular meta-heuristic, and is used to generate high-quality solutions to optimization and search problems. It relies of biologically-inspired operators such as mutation, crossover and selection.

In a genetic algorithm, a population of candidate solutions (`individuals`) to an optimization problem are generated, then evolved towards better solutions. Each candidate solution has a set of properties called chromosomes. The nature of the chromosomes are problem-dependent (i.e. they differ from problem to problem). Traditionally however, chromosomes are represented in binary as strings of 0s and 1s.

The evolution starts with a population of randomly generated individuals, and in an iterative process where the population in each iteration is referred to as a generation, the fitness of every individual in a generation is calculated. Fitness refers to the usefulness or quality of an individual. It is a value assigned to that individual that indicates how well the individual solves the intended problem. A Fitness value is usually a `double` i.e. decimal value .... However, it is possible to use other representations.

![Genetic Algorithm Flowchart](https://www.researchgate.net/profile/Hongfang_Liu/publication/260377604/figure/fig2/AS:213452158181378@1427902368463/Genetic-Algorithm-Tree-Basic-steps-of-GA-selection-crossover-and-mutation.png)

Check out the [Wikipedia Article](https://en.wikipedia.org/wiki/Genetic_algorithm) on Genetic Algorithms