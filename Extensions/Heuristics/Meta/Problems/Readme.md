# Meta-Heuristics Problems

This section contains classes, each defining a single NP-Hard Problem to be solved using the Meta Heuristic Algorithms implemented [here](https://github.com/mykeels/Extensions/new/master/Extensions/Heuristics/Meta/)

This Project contains classes for the following meta-heuristics algorithms:

- [Artificial Bee Colony Algorithm (ABC)](https://github.com/mykeels/Extensions/new/master/Extensions/Heuristics/Meta/Abc)
- [Genetic Algorithm (GA)](https://github.com/mykeels/Extensions/new/master/Extensions/Heuristics/Meta/GA)

The researcher aims to provide the following meta-heuristics algorithms soon:

- Simulated Annealing
- Particle Swarm Optimization
- Binary Pigeon Inspired Optimization
- Fireworks Optimization

## Eight Queens Chess Problem

Check out Wiki's Description for the [Eight Queens Chess Problem](https://en.wikipedia.org/wiki/Eight_queens_puzzle)

Simply, it's how do you fit 8 queens on an 8x8 chess board such that none threatens any of the others. There are only 92 perfect solutions to this problem. Oh, and 4,426,165,368 imperfect solutions. Have fun finding the perfect ones.

To test your solutions, check out this [demo](https://mykeels.github.io/Eight-Queen-Solution-Test/) which uses this [angular directive](https://github.com/mykeels/Eight-Queen-Solution-Test)

## Travelling Salesman Problem (Coming Soon)

Check out Wiki's Description for the [Travelling Salesman Problem](https://en.wikipedia.org/wiki/Travelling_salesman_problem)

This problems presents a fictional salesman who has to visit multiple cities and return to his starting city. He needs to find the shortest route through all his sales point, and has asked you to do this for him.

## Multiple Knapsack Problem (Coming Soon)

A combinatorial NP-Hard Problem, the [multiple knapsack problem](https://en.wikipedia.org/wiki/Knapsack_problem) is about choosing an optimal selection of stuff, such that they can all fit into multiple containers of different sizes. The goal is to maximize the total value of the stuff selected.

## Office Space Allocation (Coming Soon)

Another combinatorial NP-Hard Problem, the [office space allocation problem](http://www.cs.nott.ac.uk/~pszjds/research/spaceallocation.html) is about allocating rooms to resources in such a way as to maximize space use and minimize space wastage, while attempting to satisfy multiple constraints. 

- Resources: These are usually university staff who require office space in a faculty, but it could apply to staff of any organization who require office space. Each resource has a space requirement, and may have multiple constraints such as whether the resource may share a room with another.

- Rooms: These are office spaces to be allocated to resources. Each room has an available space property. A room may be shared by multiple resources.

- Constraints: These are requirements that either must or should be met. Some are called hard constraints and they must be obeyed for a solution to even be considered valid. Solutions should endeavour to meet soft constraint requirements too, even though it isn't compulsory, but it affects the solution's score.
  
  Constraints include:
    - Sharing Constraints on Resources tell us whether or not a Resource can share a room with another
    - Adjacent Room Constraints specify that two resources must be placed in adjacent rooms
    - Away From Constraints specify that two resources should not be in rooms that are close to each other
    - Many other Constraint Types exists ....


## Flowshop Scheduling (Coming Soon)

Check out Wiki's description of [FlowShop Scheduling](https://en.wikipedia.org/wiki/Flow_shop_scheduling).

## Bin Packing Problem (Coming Soon)

Check out Wiki's description of the [bin packing problem](https://en.wikipedia.org/wiki/Bin_packing_problem).

#### The Aim of this project is to make solving most problems requiring meta-heuristics as easy as possible by making use of Generic Classes that implement meta-heuristic algorithms.
