namespace CookieClijkstra
{
    public struct Path
    {
        public Vertex To;
        public float Cost;

        public string Name;

        public Path(Vertex to, float distance, string name)
        {
            this.To = to;
            this.Cost = distance;
            this.Name = name;
        }

        public override string ToString() => $"{this.Name} ({this.Cost:N2})";
    }
}