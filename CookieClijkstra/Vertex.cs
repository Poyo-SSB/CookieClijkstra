using System;

namespace CookieClijkstra
{
    public class Vertex
    {
        public FibonacciHeapNode Node { get; set; }
        public State State { get; set; }
        public float DistanceFromSource { get; set; } = Single.PositiveInfinity;

        public Vertex Previous { get; set; }
        public string PreviousName { get; set; }

        public Vertex(FibonacciHeapNode node = null, State state = default(State))
        {
            this.Node = node;
            this.State = state;
        }

        public override string ToString() => $"{this.PreviousName} => {this.DistanceFromSource:N2}";
    }
}