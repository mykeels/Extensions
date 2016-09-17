# Artificial Bee Colony Classes

![Bee Hive](http://www.cbronline.com/Uploads/NewsArticle/4928042/main.gif)

The Hive and Bee clases exist in a Hive-contains-Bees relationship. Together, they can work together to provide approximate solutions to NP-Hard problems, once the following parameters for said problems are provided:

- *Initialize Solution Function*

  This generates a new candidate solution to the problem to be solved. A candidate solution is an imperfect solution. It is an approximation of the perfect solution to the problem. E.g. in the [Eight Queens Chess Problem](https://mykeels.github.io/Eight-Queen-Solution-Test/), a random arrangement of eight queens on the chess board would be an imperfect, thus a candidate solution
  
- *Mutation (Neighbor-Finding) Function*

  This takes a solution and finds it neighbor, usually by tweaking a subset of the solution. It is advisable to clone the solution to be mutated before tweaking.
  
- *Objective Function*

  How do you find out how close or far a candidate solution is from the ideal solution? With an objective function, that is. This should always return a double that could increase when the solution gets better for a divergent approximation, or reduce when the solution gets better for a an optimization approximation. This differs based on problem, and the solution representation.
  
- *Clone Function*

  This is important to prevent multiple references pointing to the same candidate solution. Candidate Solutions are cloned before mutation. The cloning mechanics would differ based on the candidate solution class type.

- *Selection Function*

  The selection function takes in a list of solutions and their objective costs or fitnesses, and selects a specified number of solutions based on some criteria.
  
  An Example showing how to use ABC to solve the Eight Queens Problem:
  
```cs
  Configuration<byte[]> config = new Configuration<byte[]>();
  config.CloneFunction = EightQueens.Clone;
  config.InitializeSolutionFunction = EightQueens.GenerateNewCandidateSolution;
  config.MutationFunction = EightQueens.FindNeighbor;
  config.NoOfIterations = 1500;
  config.ObjectiveFunction = EightQueens.GetSolutionFitness;
  config.SelectionFunction = Selection.RoulleteWheel;
  
  Hive<byte[], Bee<byte[]>> hive = new Hive<byte[], Bee<byte[]>>();
  hive.Create(config);
  byte[] food = (byte[])hive.FullIteration();
```
  
The program above solves the eight queens problem using the artificial bee colony algorithm. Cool, yea??? Just wait till i implement the [Travelling Salesman Problem](https://simple.wikipedia.org/wiki/Travelling_salesman_problem).

![Artificial Bee Colony Algorithm](http://www.clownsonrounds.com/wp-content/uploads/2015/03/bee-cartoon.png)

## About Artificial Bee Colony

This is an optimization algorithm based on the intelligent foraging behaviour of honey bee swarm. It was proposed by Karaboga in 2005, and models the way bees look for the best food sources while foraging.

Bees are divided into:

- Employed Bees
  They have food sources (candidate solutions) as targets of their work, and they try to improve the solution. If the solution does not improve after a specifed number of trials, they abandon the food and become onlooker bees

- Onlooker Bees
  These bees watch the employed bees, and select one based on its fitness probability and the chosen selection method which is usually Roulette Wheel. When a selection is made, the onlooker bee becomes an employed bee.

- Scout Bees
  Abandoned fod sources are collected, investigated and replaced with new ones by scouts.
