using System.Collections.Generic;

namespace CookieClijkstra
{
    public class Graph
    {
        private struct Possibility
        {
            public string Name;
            public int Cost;
            public State State;

            public Possibility(string name, int cursorCost, State state)
            {
                this.Name = name;
                this.Cost = cursorCost;
                this.State = state;
            }
        }

        public int TargetDucks { get; }

        public readonly Vertex target;

        private readonly Dictionary<State, Vertex> vertices;

        private readonly FibonacciHeap queue;

        public bool Solved { get; private set; }
        public int StepCount { get; private set; }

        public Graph(int targetDucks)
        {
            this.TargetDucks = targetDucks;

            this.queue = new FibonacciHeap();

            this.target = new Vertex
            {
                PreviousName = "target"
            };

            var source = new Vertex
            {
                MinimumCost = 0
            };

            source.Node = this.queue.Insert(source, 0);
            this.vertices = new Dictionary<State, Vertex>();
        }

        public IEnumerable<Path> GetNeighborsOf(Vertex from)
        {
            State state = from.State;

            var possibilities = new List<Possibility>
            {
                new Possibility("buy foo", state.FooCost, new State(state) { Foos = (byte)(state.Foos + 1) }),
                new Possibility("buy bar", state.BarCost, new State(state) { Bars = (byte)(state.Bars + 1) }),
            };

            if (state.FooUpgradeAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy foo upgrade", 
                    State.COST_FOO_UPGRADE,
                    new State(state) { FooUpgrade = true }));
            }
            if (state.BarUpgradeAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy bar upgrade",
                    State.COST_BAR_UPGRADE,
                    new State(state) { BarUpgrade = true }));
            }

            float production = state.ProductionRate;
            foreach (Possibility possibility in possibilities)
            {
                if (!this.vertices.TryGetValue(possibility.State, out Vertex to))
                {
                    to = new Vertex(null, possibility.State);
                    this.vertices[possibility.State] = to;
                }

                yield return new Path(to, possibility.Cost / production, possibility.Name);
            }

            yield return new Path(this.target, (this.TargetDucks - state.AlreadySpent) / production, "target");
        }

        public void Step()
        {
            this.StepCount++;

            if (this.queue.IsEmpty)
            {
                this.Solved = true;
                return;
            }

            var current = this.queue.Pop();

            if (current.Vertex == this.target)
            {
                this.Solved = true;
                return;
            }

            foreach (Path neighbor in this.GetNeighborsOf(current.Vertex))
            {
                float alt = current.Vertex.MinimumCost + neighbor.Cost;
                if (alt < neighbor.To.MinimumCost)
                {
                    neighbor.To.MinimumCost = alt;
                    neighbor.To.Previous = current.Vertex;
                    neighbor.To.PreviousName = neighbor.Name;
                    if (neighbor.To.Node == null)
                    {
                        neighbor.To.Node = this.queue.Insert(neighbor.To, alt);
                    }
                    else
                    {
                        this.queue.DecreaseCost(neighbor.To.Node, alt);
                    }
                }
            }
        }

        public IEnumerable<Vertex> GetSolution()
        {
            var path = new List<Vertex>();

            Vertex current = this.target;

            while (current != null)
            {
                path.Add(current);
                current = current.Previous;
            }

            for (int i = path.Count - 1; i >= 0; i--)
            {
                yield return path[i];
            }
        }
    }
}
