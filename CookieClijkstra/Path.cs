namespace CookieClijkstra
{
    public struct Path
    {
        public Vertex To;
        public float Distance;

        public string Name;

        public Path(Vertex to, float distance, string name)
        {
            this.To = to;
            this.Distance = distance;
            this.Name = name;
        }

        public override string ToString() => $"{this.Name} ({this.Distance:N2})";
    }
}