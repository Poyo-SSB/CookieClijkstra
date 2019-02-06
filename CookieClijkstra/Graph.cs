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

        public int TargetCookies { get; }

        public readonly Vertex target;

        private readonly Dictionary<State, Vertex> vertices;

        private readonly FibonacciHeap<Vertex, double> queue;

        public bool Solved { get; private set; }
        public int StepCount { get; private set; }

        public Graph(int targetCookies)
        {
            this.TargetCookies = targetCookies;

            this.queue = new FibonacciHeap<Vertex, double>(0);

            this.target = new Vertex
            {
                PreviousName = "target"
            };

            var source = new Vertex
            {
                DistanceFromSource = 0
            };

            source.Node = this.queue.Insert(source, 0);
            this.vertices = new Dictionary<State, Vertex>();
        }

        public IEnumerable<Path> GetNeighborsOf(Vertex from)
        {
            State state = from.State;

            var possibilities = new List<Possibility>
            {
                new Possibility("buy cursor", state.CursorCost, new State(state) { Cursors = (byte)(state.Cursors + 1) }),
                new Possibility("buy grandma", state.GrandmaCost, new State(state) { Grandmas = (byte)(state.Grandmas + 1) }),
                new Possibility("buy farm", state.FarmCost, new State(state) { Farms = (byte)(state.Farms + 1) }),
                new Possibility("buy mine", state.MineCost, new State(state) { Mines = (byte)(state.Mines + 1) }),
                new Possibility("buy factory", state.FactoryCost, new State(state) { Factories = (byte)(state.Factories + 1) })
            };

            if (state.ReinforcedIndexFingerAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy reinforced index finger", 
                    State.COST_REINFORCED_INDEX_FINGER,
                    new State(state) { ReinforcedIndexFinger = true }));
            }
            if (state.CarpalTunnelPreventionCreamAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy carpal tunnel prevention cream",
                    State.COST_CARPAL_TUNNEL_PREVENTION_CREAM,
                    new State(state) { CarpalTunnelPreventionCream = true }));
            }
            if (state.AmbidextrousAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy ambidextrous",
                    State.COST_AMBIDEXTROUS,
                    new State(state) { Ambidextrous = true }));
            }
            if (state.ThousandFingersAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy thousand fingers",
                    State.COST_THOUSAND_FINGERS,
                    new State(state) { ThousandFingers = true }));
            }

            if (state.ForwardsFromGrandmaAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy forwards from grandma",
                    State.COST_FORWARDS_FROM_GRANDMA,
                    new State(state) { ForwardsFromGrandma = true }));
            }
            if (state.SteelPlatedRollingPinsAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy steel plated rolling pins",
                    State.COST_STEEL_PLATED_ROLLING_PINS,
                    new State(state) { SteelPlatedRollingPins = true }));
            }
            if (state.LubricatedDenturesAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy lubricated dentures",
                    State.COST_LUBRICATED_DENTURES,
                    new State(state) { LubricatedDentures = true }));
            }

            if (state.CheapHoesAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy cheap hoes",
                    State.COST_CHEAP_HOES,
                    new State(state) { CheapHoes = true }));
            }
            if (state.FertilizerAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy fertilizer",
                    State.COST_FERTILIZER,
                    new State(state) { Fertilizer = true }));
            }
            if (state.CookieTreesAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy cookies trees",
                    State.COST_COOKIE_TREES,
                    new State(state) { CookieTrees = true }));
            }

            if (state.SugarGasAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy sugar gas",
                    State.COST_SUGAR_GAS,
                    new State(state) { SugarGas = true }));
            }
            if (state.MegadrillAvailable)
            {
                possibilities.Add(new Possibility(
                    "buy megadrill",
                    State.COST_MEGADRILL,
                    new State(state) { Megadrill = true }));
            }

            double cps = state.CookiesPerSecond;
            foreach (Possibility possibility in possibilities)
            {
                if (!this.vertices.TryGetValue(possibility.State, out Vertex to))
                {
                    to = new Vertex(null, possibility.State);
                    this.vertices[possibility.State] = to;
                }

                yield return new Path(to, possibility.Cost / cps, possibility.Name);
            }

            yield return new Path(this.target, (this.TargetCookies - state.CookiesAlreadySpent) / cps, "target");
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

            if (current.Data == this.target)
            {
                this.Solved = true;
                return;
            }

            foreach (Path neighbor in this.GetNeighborsOf(current.Data))
            {
                double alt = current.Data.DistanceFromSource + neighbor.Distance;
                if (alt < neighbor.To.DistanceFromSource)
                {
                    neighbor.To.DistanceFromSource = alt;
                    neighbor.To.Previous = current.Data;
                    neighbor.To.PreviousName = neighbor.Name;
                    if (neighbor.To.Node == null)
                    {
                        neighbor.To.Node = this.queue.Insert(neighbor.To, alt);
                    }
                    else
                    {
                        this.queue.DecreaseKey(neighbor.To.Node, alt);
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
