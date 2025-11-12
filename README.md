# Evolution-Strategy-Simulation
This project is a simulation of an Evolution Strategy (ES) applied to a population of agents that learn to survive in a Flappy Bird-like environment.

Each agent is represented by a simple neural network that decides whether to move up or down to avoid obstacles. Over generations, the population evolves and the most successful agents reproduce and pass on their weights with slight mutations, leading to improved performance over time.


https://github.com/user-attachments/assets/67276604-f36b-4842-9045-93baf8744650

For the simulator’s design, I mimicked [Dani’s Fa**y Rocket game](https://www.youtube.com/watch?v=EGBvvlgbJVM) since it can be recreated in Unity with Post Processing Effects in just a few minutes and looks cool too!

## How It Works?

The simulation models a biologically inspired learning process where no gradient-based training (like backpropagation) is used.
Instead, it relies purely on evolutionary principles:

1. **Initialization**: Create a population of agents, each with a neural network whose weights are randomly initialized.

2. **Evaluation**: Each agent interacts with the environment. Its fitness score is determined by how long it survives (i.e., how far it progresses before hitting an obstacle).

3. **Selection**: After all agents die, the top 50% of the population (those with the highest fitness) are selected as parents.

4. **Reproduction and Mutation**: The parents are copied and mutated slightly to form a new generation.

5. **Repeat**: This process continues for multiple generations until the agents consistently perform well.

<div align="center">
  <img width="372" height="923" alt="Screenshot 2025-11-12 143452" src="https://github.com/user-attachments/assets/1645820d-ab0a-46fa-aa70-b9670da13e2d" />
</div>

## Simulator Features

- **Adjustable Population Size**: set the total number of agents per simulation.

- **Save & Load**: store and reload simulations to continue or analyze previous runs.

- **Network Visualization**: right-click an agent to view its neural network structure in real time.

<div align="center">
  <img width="1920" height="915" alt="Screenshot 2025-11-12 132313" src="https://github.com/user-attachments/assets/9a490752-66f8-44da-b097-dd4398f228d7" />
</div>
